using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Versus.Dtos;
using Versus.Dtos.Tournament;
using Versus.Services;

namespace Versus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentsController : ControllerBase
    {
        private readonly TournamentService _tournamentService;

        public TournamentsController(TournamentService tournamentService)
        {
            _tournamentService = tournamentService;
        }


        // POST: api/Tournaments
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> InsertTournament([FromBody] InsertTournamentDto dto)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId == null) return Unauthorized(new ResponseDto { Success = false, Message = "Usuário inválido!" });

                await _tournamentService.InsertTournamentAsync(dto, int.Parse(userId));

                return Created(string.Empty, new ResponseDto { Success = true, Message = "Torneio criado com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto { Success = false, Message = ex.Message });
            }
        }

        // GET: api/Tournaments
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<TournamentDetailsDto>>> GetCurrentUserTournaments()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null) return Unauthorized(new ResponseDto { Success = false, Message = "Usuário inválido!" });

                IEnumerable<TournamentDetailsDto> tournament = await _tournamentService.GetCurrentUserTournamentsAsync(int.Parse(userId));

                return Ok(new ResponseDto { Success = true, Message = "Seus torneios foram encontrados!", Data = tournament });

            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto { Success = false, Message = ex.Message });
            }
        }

        // Get: api/Tournaments/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetTournamentById(int id)
        {
            try
            {
                TournamentDetailsDto tournament = await _tournamentService.GetTournamentByIdAsync(id);
                return Ok(new ResponseDto { Success = true, Message = "Torneio encontrado!", Data = tournament });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto { Success = false, Message = ex.Message });
            }
        }

        // PATCH: api/Tournaments/5
        [HttpPatch("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateTournament([FromBody] UpdateTournamentDto dto, int id)
        {
            try
            {
                await _tournamentService.UpdateTournamentAsync(dto, id);
                return Ok(new ResponseDto { Success = true, Message = "Torneio atualizado com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto { Success = false, Message = ex.Message });
            }
        }

        // DELETE: api/Tournaments/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteTournament(int id)
        {
            try
            {
                await _tournamentService.DeleteTournamentAsync(id);
                return Ok(new ResponseDto { Success = true, Message = "Torneio excluido com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto { Success = false, Message = ex.Message });
            }
        }
    }
}
