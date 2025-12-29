using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Versus.Dtos;
using Versus.Dtos.Tournament;
using Versus.Services;

namespace Versus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly PlayerService _playerService;
        public PlayersController(PlayerService playerService)
        {
            _playerService = playerService;
        }

        // POST: api/Players/5
        [HttpPost("{tournamentId}")]
        [Authorize]
        public async Task<IActionResult> InsertPlayers([FromBody] List<InsertPlayerDto> dto, int tournamentId)
        {
            try
            {
                await _playerService.InsertPlayersAsync(dto, tournamentId);

                return Ok(new ResponseDto { Success = true, Message = "Competidores adicionados com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto { Success = false, Message = ex.Message });
            }
        }

        // PUT: api/Players/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdatePlayer([FromBody] UpdatePlayerDto dto, int id)
        {
            try
            {
                await _playerService.UpdatePlayerAsync(dto, id);
                return Ok(new ResponseDto { Success = true, Message = "Competidor atualizado com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto { Success = false, Message = ex.Message });
            }
        }

        // DELETE: api/Players/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            try
            {
                await _playerService.DeletePlayerAsync(id);
                return Ok(new ResponseDto { Success = true, Message = "Competidor removido com sucesso!" });
            }
            catch(Exception ex)
            {
                return BadRequest(new ResponseDto { Success = false, Message = ex.Message });
            }
        }
    }
}
