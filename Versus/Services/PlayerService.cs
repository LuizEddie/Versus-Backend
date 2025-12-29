using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Versus.Data;
using Versus.Dtos.Tournament;
using Versus.Models.Tournament;
using Versus.Services.Exceptions;

namespace Versus.Services
{
    public class PlayerService
    {
        private readonly VersusContext _context;
        public PlayerService(VersusContext context) { 
            _context = context;
        }
        public async Task InsertPlayersAsync(List<InsertPlayerDto> dto, int tournamentId)
        {
            List<Player> players = dto.Select(item => new Player() {  
                Name = item.Name,
                TournamentId = tournamentId
            }).ToList();

            await _context.Players.AddRangeAsync(players);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePlayerAsync(UpdatePlayerDto dto, int id)
        {
            Player player = await _context.Players.FirstOrDefaultAsync(x => x.Id == id) ?? throw new TournamentException("Competidor não encontrado!");
            try
            {
                player.Name = dto.Name;
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException ex)
            {
                throw new DbConcurrencyException(ex.Message);
            }
        }

        public async Task DeletePlayerAsync(int id)
        {
            Player player = await _context.Players.FirstOrDefaultAsync(x => x.Id == id) ?? throw new TournamentException("Competidor não encontrado");

            _context.Players.Remove(player);
            await _context.SaveChangesAsync();


        }
    }
}
