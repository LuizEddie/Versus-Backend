using Microsoft.EntityFrameworkCore;
using Versus.Data;
using Versus.Dtos.Tournament;
using Versus.Models.Tournament;

namespace Versus.Services
{
    public class TournamentWinConditionService
    {
        private readonly VersusContext _context;

        public TournamentWinConditionService(VersusContext context)
        {
            _context = context;
        }

        public async Task<List<TournamentWinConditionDto>> getTournamentWinConditionsAsync()
        {
            List<TournamentWinConditon> winConditionList = await _context.TournamentWinConditon.ToListAsync();

            List<TournamentWinConditionDto> winConditionDto = winConditionList.Select(t => new TournamentWinConditionDto { Id = t.Id, Name = t.Name }).ToList();

            return winConditionDto;
        }
    }
}
