using Microsoft.EntityFrameworkCore;
using Versus.Data;
using Versus.Dtos.Tournament;
using Versus.Models.Tournament;

namespace Versus.Services
{
    public class TournamentStatusService
    {
        private readonly VersusContext _context;

        public TournamentStatusService(VersusContext context)
        {
            _context = context;
        }

        public async Task<List<TournamentStatusDto>> getTournamentStatusAsync()
        {
            List<TournamentStatus> statusList = await _context.TournamentStatuses.ToListAsync();

            List<TournamentStatusDto> statusListDto = statusList.Select(t => new TournamentStatusDto { Id = t.Id, Name = t.Name }).ToList();

            return statusListDto;
        }
    }
}
