using Microsoft.EntityFrameworkCore;
using Versus.Data;
using Versus.Dtos.Tournament;
using Versus.Models.Tournament;

namespace Versus.Services
{
    public class TournamentTieBreakerService
    {
        private readonly VersusContext _context;

        public TournamentTieBreakerService(VersusContext context)
        {
            _context = context;
        }

        public async Task<List<TournamentTieBreakerDto>> getTournamentTieBreaker()
        {
            List<TournamentTieBreaker> tieBreakerList = await _context.TournamentTieBreaker.ToListAsync();

            List<TournamentTieBreakerDto> tieBreakerDto = tieBreakerList.Select(t => new TournamentTieBreakerDto { Id = t.Id, Name = t.Name }).ToList();

            return tieBreakerDto;
        }
    }
}
