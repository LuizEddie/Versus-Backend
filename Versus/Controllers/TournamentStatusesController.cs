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
    public class TournamentStatusesController : ControllerBase
    {
        private readonly TournamentStatusService _statusService;

        public TournamentStatusesController(TournamentStatusService statusService) { 
            _statusService = statusService;
        }

        // GET: api/TournamentStatus
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<TournamentStatusDto>>> getTournamentStatuses()
        {
            try
            {
                IEnumerable<TournamentStatusDto> tournamentStatuses = await _statusService.getTournamentStatusAsync();
                return Ok(new ResponseDto { Success = true, Message = "Dados carregados com sucesso", Data = tournamentStatuses});
            }
            catch (Exception ex) { 
                return BadRequest(new ResponseDto { Success = false, Message = ex.Message});
            }
        }
    }
}
