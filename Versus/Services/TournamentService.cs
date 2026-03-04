using Microsoft.EntityFrameworkCore;
using Versus.Data;
using Versus.Dtos.Tournament;
using Versus.Models;
using Versus.Models.Tournament;
using Versus.Services.Exceptions;

namespace Versus.Services
{
    public class TournamentService
    {
        private readonly VersusContext _context;
        public TournamentService(VersusContext context)
        {
            _context = context;
        }

        public async Task<TournamentDetailsDto> InsertTournamentAsync(InsertTournamentDto dto, int userId)
        {
            if (!dto.TournamentSpec.TypeId.HasValue || dto.TournamentSpec.TypeId.Value == 0) throw new TournamentException("O tipo de torneio é obrigatório");

            if (!await _context.TournamentTypes.AnyAsync(x => x.Id == dto.TournamentSpec.TypeId.Value)) throw new TournamentException("Tipo de torneio inexistente, por favor selecione outro");

            Tournament tournament = new()
            {
                Name = dto.Name,
                UserId = userId,
                Players = dto.Players?.Select(p => new Player { Name = p.Name }).ToList(),
                TournamentSpec = new TournamentSpec
                {
                    WinConditionId = dto.TournamentSpec.WinConditionId,
                    TypeId = dto.TournamentSpec.TypeId.Value
                }
            };

            _context.Tournaments.Add(tournament);
            await _context.SaveChangesAsync();
            TournamentDetailsDto createdTournament = new TournamentDetailsDto { Id = tournament.Id, Name = tournament.Name };
            return createdTournament;
        }

        public async Task<List<TournamentDetailsDto>> GetCurrentUserTournamentsAsync(int userId)
        {
            var tournaments = await _context.Tournaments
                .Include(x => x.Players)
                .Include(x => x.TournamentSpec)
                    .ThenInclude(x => x.Type)
                .Include(x => x.TournamentSpec)
                    .ThenInclude(x => x.WinCondition)
                .Include(x => x.User)
                .Include(x => x.Status)
                .Include(x => x.Matches)
                .Include(x => x.Winner)
                .Where(x => x.UserId == userId)
                .Where(x => x.IsDeleted == false)
                .ToListAsync();

            List<TournamentDetailsDto> tournamentDetailsDtos = tournaments.Select(t => new TournamentDetailsDto
            {
                Id = t.Id,
                Name = t.Name,
                CreatedAt = t.CreatedAt,
                FinishedAt = t.FinishedAt,
                UserDetails = new Dtos.User.UserDetailsDto
                {
                    Id = t.User.Id,
                    Name = t.User.Name,
                    Email = t.User.Email,
                    PhotoPath = t.User.PhotoPath
                },
                WinnerDetails = t.Winner == null ? null : new PlayerDetailsDto
                {
                    Id = t.Winner.Id,
                    Name = t.Winner.Name,
                },
                PlayerDetails = t.Players.Select(p => new PlayerDetailsDto
                {
                    Id = p.Id,
                    Name = p.Name
                }).ToList(),
                TournamentSpecs = new TournamentSpecsDetailsDto
                {
                    Id = t.TournamentSpec.Id,
                    WinCondition = t.TournamentSpec.WinCondition,
                    Type = t.TournamentSpec.Type
                },
                TournamentStatus = t.Status,
                MatchDetails = t.Matches.Select(m => new MatchDetailsDto
                {
                    Id = m.Id,
                    MatchDate = m.MatchDate,
                    Player1 = m.Player1 == null ? null : new PlayerDetailsDto { Id = m.Player1.Id, Name = m.Player1.Name },
                    Player2 = m.Player2 == null ? null : new PlayerDetailsDto { Id = m.Player2.Id, Name = m.Player2.Name },
                    Round = m.Round,
                    Winner = m.Winner == null ? null : new PlayerDetailsDto { Id = m.Winner.Id, Name = m.Winner.Name },
                    Key = m.Key
                }).ToList()
            }).ToList();

            return tournamentDetailsDtos;
        }

        public async Task<TournamentDetailsDto> GetTournamentByIdAsync(int id)
        {
            var tournament = await _context.Tournaments
               .Include(x => x.Players)
               .Include(x => x.TournamentSpec)
                   .ThenInclude(x => x.Type)
               .Include(x => x.TournamentSpec)
                    .ThenInclude(x => x.WinCondition)
               .Include(x => x.User)
               .Include(x => x.Status)
               .Include(x => x.Matches)
                    .ThenInclude(x => x.Player1)
                .Include(x => x.Matches)
                    .ThenInclude(x => x.Player2)
                .Include(x => x.Matches)
                    .ThenInclude(x => x.Winner)
                .Include(x => x.Winner)
               .Where(x => x.Id == id)
               .Where(x => x.IsDeleted == false)
               .FirstOrDefaultAsync() ?? throw new TournamentException("Torneio não encontrado");

            TournamentDetailsDto tournamentDetailsDto = new TournamentDetailsDto
            {
                Id = tournament.Id,
                Name = tournament.Name,
                UserDetails = new Dtos.User.UserDetailsDto
                {
                    Id = tournament.User.Id,
                    Name = tournament.User.Name,
                    Email = tournament.User.Email,
                    PhotoPath = tournament.User.PhotoPath
                },
                PlayerDetails = tournament.Players.Select(p => new PlayerDetailsDto
                {
                    Id = p.Id,
                    Name = p.Name
                }).OrderBy(p => p.Id).ToList(),
                WinnerDetails = tournament.Winner == null ? null : new PlayerDetailsDto 
                {
                  Id = tournament.Winner.Id,
                  Name = tournament.Winner.Name,
                },
                TournamentSpecs = new TournamentSpecsDetailsDto
                {
                    Id = tournament.TournamentSpec.Id,
                    WinCondition = tournament.TournamentSpec.WinCondition,
                    Type = tournament.TournamentSpec.Type,
                },
                TournamentStatus = tournament.Status,
                MatchDetails = tournament.Matches.Select(m => new MatchDetailsDto
                {
                    Id = m.Id,
                    MatchDate = m.MatchDate,
                    Player1 = m.Player1 == null ? null : new PlayerDetailsDto { Id = m.Player1.Id, Name = m.Player1.Name },
                    Player2 = m.Player2 == null ? null : new PlayerDetailsDto { Id = m.Player2.Id, Name = m.Player2.Name },
                    Round = m.Round,
                    Winner = m.Winner == null ? null : new PlayerDetailsDto { Id = m.Winner.Id, Name = m.Winner.Name },
                    Key = m.Key
                }).ToList()
            };

            return tournamentDetailsDto;
        }

        public async Task UpdateTournamentAsync(UpdateTournamentDto dto, int id)
        {
            Tournament tournament = await _context.Tournaments.FirstOrDefaultAsync(t => t.Id == id) ?? throw new TournamentException("Torneio não encontrado!");

            try
            {
                tournament.UpdatedAt = DateTime.UtcNow;
                if (!string.IsNullOrEmpty(dto.Name)) tournament.Name = dto.Name;
                if (dto.WinnerId.HasValue) tournament.WinnerId = dto.WinnerId.Value;
                if (dto.StatusId.HasValue) tournament.StatusId = dto.StatusId.Value;
                if (dto.StatusId.HasValue && dto.StatusId.Value == 2) tournament.FinishedAt = DateTime.UtcNow;
                if (dto.StatusId.HasValue && dto.StatusId.Value == 0) throw new TournamentException("Status do torneio é obrigatório!");
                if (dto.StatusId.HasValue && dto.StatusId.Value == 2 && !dto.WinnerId.HasValue) throw new TournamentException("O vencedor é obrigatório!");

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new DbUpdateException(ex.Message);
            }
        }

        public async Task DeleteTournamentAsync(int id)
        {
            Tournament tournament = await _context.Tournaments.FirstOrDefaultAsync(t => t.Id == id) ?? throw new TournamentException("Torneio não encontrado!");
            try
            {
                tournament.IsDeleted = true;
                tournament.DeletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex) {
                throw new DbUpdateException(ex.Message);
            }
        }
    }
}
