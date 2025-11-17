using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Versus.Dtos.Tournament;
using Versus.Dtos;
using Versus.Services;

namespace Versus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentWinConditionsController : ControllerBase
    {
        private readonly TournamentWinConditionService _tournamentWinConditionService;

        public TournamentWinConditionsController(TournamentWinConditionService tournamentWinConditionService)
        {
            _tournamentWinConditionService = tournamentWinConditionService;
        }

        // GET: api/TournamentWinCondition
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<TournamentWinConditionDto>>> getTournamentWinConditionsAsync()
        {
            try
            {
                IEnumerable<TournamentWinConditionDto> winConditionDtos = await _tournamentWinConditionService.getTournamentWinConditionsAsync();
                return Ok(new ResponseDto { Message = "Dados obtidos com sucesso!", Success = true, Data = winConditionDtos });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto { Success = false, Message = ex.Message });
            }
        }
    }
}
