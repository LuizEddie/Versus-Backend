using Microsoft.EntityFrameworkCore;
using Versus.Data;
using Versus.Dtos.Tournament;
using Versus.Models.Tournament;
using Versus.Services.Exceptions;

namespace Versus.Services
{
    public class TournamentService
    {
        private readonly VersusContext _context;
        private readonly UserService _userService;
        public TournamentService(VersusContext context, UserService userService) {
            _context = context;
            _userService = userService;
        }

        public async Task InsertTournamentAsync(InsertTournamentDto dto, int userId)
        {
            Tournament tournament = new ()
            {
                Name = dto.Name,
                UserId = userId
            };

            _context.Tournaments.Add(tournament);
            await _context.SaveChangesAsync();

            var insertedId = tournament.Id;

            if (dto.Players != null && dto.Players.Any())
            {
                List<Player> players = dto.Players.Select(p => new Player
                {
                    Name = p.Name,
                    TournamentId = insertedId
                }).ToList();


                _context.Players.AddRange(players);
                await _context.SaveChangesAsync();
            }

            if (!dto.TournamentSpec.TypeId.HasValue || dto.TournamentSpec.TypeId.Value == 0) throw new TournamentException("O tipo de torneio é obrigatório");

            if (!await _context.TournamentTypes.AnyAsync(x => x.Id == dto.TournamentSpec.TypeId.Value)) throw new TournamentException("Tipo de torneio inexistente, por favor selecione outro");

            TournamentSpec tournamentSpec = new()
            {
                MatchDuration = dto.TournamentSpec.MatchDuration,
                PointsPerDraw = dto.TournamentSpec.PointsPerDraw,
                PointsPerLose = dto.TournamentSpec.PointsPerLose,
                PointsPerWin = dto.TournamentSpec.PointsPerWin,
                QtyRounds = dto.TournamentSpec.QtyRounds,
                TieBreaker = dto.TournamentSpec.TieBreaker,
                WinCondition = dto.TournamentSpec.WinCondition,
                TournamentId = insertedId
            };

            tournamentSpec.TypeId = dto.TournamentSpec.TypeId.Value;

            _context.TournamentSpecs.Add(tournamentSpec);
            await _context.SaveChangesAsync();
        } 
    }
}
