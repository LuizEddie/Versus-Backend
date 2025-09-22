using Microsoft.EntityFrameworkCore;
using Versus.Data;
using Versus.Dtos.Tournament;
using Versus.Models.Tournament;
using Versus.Services.Exceptions;

namespace Versus.Services
{
    public class TournamentSpecService
    {
        private readonly VersusContext _context;

        public TournamentSpecService(VersusContext context)
        {
            _context = context;
        }

        public async Task UpdateSpecAsync(UpdateTournamentSpecsDto dto, int id)
        {
            TournamentSpec tournamentSpec = await _context.TournamentSpecs.FirstOrDefaultAsync(x => x.Id == id) ?? throw new TournamentException("Especificações não encontradas!");

            try
            {
                if (dto.PointsPerWin.HasValue) tournamentSpec.PointsPerWin = dto.PointsPerWin.Value;
                if (dto.PointsPerDraw.HasValue) tournamentSpec.PointsPerDraw = dto.PointsPerDraw.Value;
                if (dto.PointsPerLose.HasValue) tournamentSpec.PointsPerLose = dto.PointsPerLose.Value;
                if (dto.TypeId.HasValue) tournamentSpec.TypeId = dto.TypeId.Value;
                if (dto.QtyRounds.HasValue) tournamentSpec.QtyRounds = dto.QtyRounds.Value;
                if (dto.WinConditionId.HasValue) tournamentSpec.WinConditionId = dto.WinConditionId.Value;
                if (dto.TieBreakerId.HasValue) tournamentSpec.TieBreakerId = dto.TieBreakerId.Value;
                if (dto.MatchDuration.HasValue) tournamentSpec.MatchDuration = dto.MatchDuration.Value; 

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new DbConcurrencyException(ex.Message);
            }
        }
    }
}
