using Microsoft.EntityFrameworkCore;
using Versus.Data;
using Versus.Dtos.Tournament;
using Versus.Models.Tournament;

namespace Versus.Services
{
    public class TournamentTypeService
    {
        private readonly VersusContext _context;

        public TournamentTypeService(VersusContext context)
        {
            _context = context;
        }

        public async Task<List<TournamentTypeDto>> getTournamentTypesAsync()
        {
            List<TournamentType> typesList = await _context.TournamentTypes.ToListAsync();

            List<TournamentTypeDto> typesListDto = typesList.Select(t => new TournamentTypeDto { Id = t.Id, Name = t.Name }).ToList();

            return typesListDto;
        }
    }
}
