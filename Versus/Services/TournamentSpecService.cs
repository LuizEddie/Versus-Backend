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
                if (dto.TypeId.HasValue) tournamentSpec.TypeId = dto.TypeId.Value;
                if (dto.WinConditionId.HasValue) tournamentSpec.WinConditionId = dto.WinConditionId.Value;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new DbConcurrencyException(ex.Message);
            }
        }
    }
}
