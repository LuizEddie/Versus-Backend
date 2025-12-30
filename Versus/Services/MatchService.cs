using Humanizer;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Cms;
using System.Text.Json;
using Versus.Data;
using Versus.Dtos.Tournament;
using Versus.Models.Tournament;
using Versus.Services.Exceptions;

namespace Versus.Services
{
    public class MatchService
    {
        private readonly VersusContext _context;

        public MatchService(VersusContext context)
        {
            _context = context;
        }

        public async Task InsertMatchAsync(InsertMatchDto dto)
        {
            Match match = new Match
            {
                Group = dto.Group,
                Phase = dto.Phase,
                Player1Id = dto.Player1Id,
                Player2Id = dto.Player2Id,
                Round = dto.Round,
                TournamentId = dto.TournamentId,
                Key = dto.Key,
            };

            _context.Matches.Add(match);
            await _context.SaveChangesAsync();
        }

        public async Task SetMatchWinnerAsync(int id, int winnerId)
        {

            var result = await _context.Matches.Where(x => x.Id == id).FirstOrDefaultAsync() ?? throw new TournamentException("Partida não encontrada!");

            try
            {
                result.WinnerId = winnerId;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new DbConcurrencyException(ex.Message);
            }
        }

        public async Task UpdateMatchAsync(int id, UpdateMatchDto dto)
        {
            var result = await _context.Matches.Where(x => x.Id == id).FirstOrDefaultAsync() ?? throw new TournamentException("Partida não encontrada!");

            try
            {
                if (!string.IsNullOrEmpty(dto.Phase)) result.Phase = dto.Phase;
                if (!string.IsNullOrEmpty(dto.Round)) result.Round = dto.Round;
                if (!string.IsNullOrEmpty(dto.Key)) result.Key = dto.Key;
                if (dto.Player1Id.HasValue) result.Player1Id = dto.Player1Id;
                if (dto.Player2Id.HasValue) result.Player2Id = dto.Player2Id;
                if (dto.WinnerId.HasValue) result.WinnerId = dto.WinnerId;
                if (dto.Group.HasValue) result.Group = dto.Group;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new DbConcurrencyException(ex.Message);
            }
        }

        public async Task CreateFirstKnockoutStageMatchesAsync(int tournamentId)
        {
            var rnd = new Random();
            var players = await _context.Players.Where(x => x.TournamentId == tournamentId).ToListAsync() ?? throw new TournamentException("Jogadores não encontrados!");
            var specs = await _context.TournamentSpecs.Where(x => x.TournamentId == tournamentId).FirstOrDefaultAsync() ?? throw new TournamentException("Especificações do torneio não encontrados!");
            int group = specs.QtyGroups == null || specs.QtyGroups == 0 ? 1 : (int)specs.QtyGroups;

            players = players.OrderBy(x => rnd.Next()).ToList();

            var parts = SplitIntoParts(players, group).ToList();

            List<Match> matches = new List<Match>();

            //calcular round
            int totalMatches = (players.Count() + 1) / 2;
            string round = totalMatches switch
            {
                1 => "Final",
                2 => "Semifinal",
                _ => $"{totalMatches}ª"
            };

            for (int i = 0; i < parts.Count(); i++)
            {
                var part = parts[i];
                for (int j = 0; j < part.Count; j += 2)
                {
                    Match match = new Match();
                    match.Player1Id = part[j].Id;
                    match.Player2Id = (j + 1 < part.Count) ? part[j + 1].Id : (int?)null;

                    match.TournamentId = tournamentId;
                    match.Group = i;
                    match.Round = round;

                    matches.Add(match);
                }
            }

            for (int i = 0; i < matches.Count(); i++)
            {
                matches[i].Key = (i + 1).ToString();
            }

            _context.Matches.AddRange(matches);
            await _context.SaveChangesAsync();
        }

        public async Task<MatchDetailsDto> FindSingleMatchAsync(MatchFilterDto dto)
        {
            var query = _context.Matches.AsQueryable();
            if (dto.Id.HasValue) query = query.Where(x => x.Id == dto.Id);
            if (!string.IsNullOrWhiteSpace(dto.Phase)) query = query.Where(x => x.Phase == dto.Phase);
            if (!string.IsNullOrWhiteSpace(dto.Round)) query = query.Where(x => x.Round == dto.Round);
            if (!string.IsNullOrWhiteSpace(dto.Key)) query = query.Where(x => x.Key == dto.Key);
            if (dto.MatchDate.HasValue) query = query.Where(x => x.MatchDate == dto.MatchDate);
            if (dto.TournamentId.HasValue) query = query.Where(x => x.TournamentId == dto.TournamentId);
            if (dto.Group.HasValue) query = query.Where(x => x.Group == dto.Group);
            if (dto.Player1Id.HasValue) query = query.Where(x => x.Player1Id == dto.Player1Id);
            if (dto.Player2Id.HasValue) query = query.Where(x => x.Player2Id == dto.Player2Id);
            if (dto.WinnerId.HasValue) query = query.Where(x => x.WinnerId == dto.WinnerId);

            var result = await query
                .Include(x => x.Winner)
                .Include(x => x.Player1)
                .Include(x => x.Player2)
                .FirstOrDefaultAsync();

            if (result == null) return null;

            return new MatchDetailsDto()
            {
                Id = result.Id,
                Phase = result.Phase,
                Round = result.Round,
                Group = result.Group,
                Key = result.Key,
                MatchDate = result.MatchDate,
                Player1 = result.Player1 != null ? new PlayerDetailsDto() { Id = result.Player1.Id, Name = result.Player1.Name } : null,
                Player2 = result.Player2 != null ? new PlayerDetailsDto() { Id = result.Player2.Id, Name = result.Player2.Name } : null,
                Winner = result.Winner != null ? new PlayerDetailsDto() { Id = result.Winner.Id, Name = result.Winner.Name } : null,
            };
        }

        public IEnumerable<List<T>> SplitIntoParts<T>(List<T> list, int parts)
        {
            int size = (int)Math.Ceiling(list.Count / (double)parts);
            for (int i = 0; i < parts; i++)
            {
                yield return list.Skip(i * size).Take(size).ToList();
            }
        }

    }
}
