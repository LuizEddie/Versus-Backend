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
        private readonly UserService _userService;
        public TournamentService(VersusContext context, UserService userService) {
            _context = context;
            _userService = userService;
        }

        public async Task InsertTournamentAsync(InsertTournamentDto dto, int userId)
        {
            if (!dto.TournamentSpec.TypeId.HasValue || dto.TournamentSpec.TypeId.Value == 0) throw new TournamentException("O tipo de torneio é obrigatório");

            if (!await _context.TournamentTypes.AnyAsync(x => x.Id == dto.TournamentSpec.TypeId.Value)) throw new TournamentException("Tipo de torneio inexistente, por favor selecione outro");

            Tournament tournament = new ()
            {
                Name = dto.Name,
                UserId = userId,
                Players = dto.Players?.Select(p => new Player { Name = p.Name}).ToList(),
                TournamentSpec = new TournamentSpec
                {
                    MatchDuration = dto.TournamentSpec.MatchDuration,
                    PointsPerDraw = dto.TournamentSpec.PointsPerDraw,
                    PointsPerLose = dto.TournamentSpec.PointsPerLose,
                    PointsPerWin = dto.TournamentSpec.PointsPerWin,
                    QtyRounds = dto.TournamentSpec.QtyRounds,
                    TieBreaker = dto.TournamentSpec.TieBreaker,
                    WinCondition = dto.TournamentSpec.WinCondition,
                    TypeId = dto.TournamentSpec.TypeId.Value
                }
            };

            _context.Tournaments.Add(tournament);
            await _context.SaveChangesAsync();
        }

        public async Task<List<TournamentDetailsDto>> GetCurrentUserTournamentsAsync(int userId)
        {
            var tournaments = await _context.Tournaments
                .Include(x => x.Players)
                .Include(x => x.TournamentSpec)
                    .ThenInclude(x => x.Type)
                .Include(x => x.User)
                .Include(x => x.Status)
                .Where(x => x.UserId == userId).ToListAsync();

            List<TournamentDetailsDto> tournamentDetailsDtos = tournaments.Select(t => new TournamentDetailsDto
            {
                Id = t.Id,
                Name = t.Name,
                UserDetails = new Dtos.User.UserDetailsDto
                {
                    Id = t.User.Id,
                    Name = t.User.Name,
                    Email = t.User.Email,
                    PhotoPath = t.User.PhotoPath
                },
                PlayerDetails = t.Players.Select(p => new PlayerDetailsDto
                {
                    Id = p.Id,
                    Name = p.Name
                }).ToList(),
                TournamentSpecs = new TournamentSpecsDetailsDto { 
                    Id = t.TournamentSpec.Id, 
                    MatchDuration = (TimeSpan)t.TournamentSpec.MatchDuration,
                    PointsPerDraw = (int)t.TournamentSpec.PointsPerDraw,
                    PointsPerLose = (int)t.TournamentSpec.PointsPerLose,
                    PointsPerWin = (int)t.TournamentSpec.PointsPerWin,
                    QtyRounds = (int)t.TournamentSpec.QtyRounds,
                    TieBreaker = t.TournamentSpec.TieBreaker,
                    WinCondition = t.TournamentSpec.WinCondition,
                    Type = t.TournamentSpec.Type
                },
                TournamentStatus = t.Status
            }).ToList();

            return tournamentDetailsDtos;
        }

        public async Task<TournamentDetailsDto> GetTournamentByIdAsync(int id)
        {
            var tournament = await _context.Tournaments
               .Include(x => x.Players)
               .Include(x => x.TournamentSpec)
                   .ThenInclude(x => x.Type)
               .Include(x => x.User)
               .Include(x => x.Status)
               .Where(x => x.Id == id)
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
                }).ToList(),
                TournamentSpecs = new TournamentSpecsDetailsDto
                {
                    Id = tournament.TournamentSpec.Id,
                    MatchDuration = (TimeSpan)tournament.TournamentSpec.MatchDuration,
                    PointsPerDraw = (int)tournament.TournamentSpec.PointsPerDraw,
                    PointsPerLose = (int)tournament.TournamentSpec.PointsPerLose,
                    PointsPerWin = (int)tournament.TournamentSpec.PointsPerWin,
                    QtyRounds = (int)tournament.TournamentSpec.QtyRounds,
                    TieBreaker = tournament.TournamentSpec.TieBreaker,
                    WinCondition = tournament.TournamentSpec.WinCondition,
                    Type = tournament.TournamentSpec.Type
                },
                TournamentStatus = tournament.Status
            };

            return tournamentDetailsDto;
        }
    }
}
