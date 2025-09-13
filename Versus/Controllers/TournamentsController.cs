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

                return Ok(new ResponseDto { Success = true, Message = "Torneio criado com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto { Success = false, Message = ex.Message });
            }
        }
    }
}
