using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Versus.Dtos.Tournament;
using Versus.Services;
using Versus.Dtos;

namespace Versus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentTieBreakerController : ControllerBase
    {
        private readonly TournamentTieBreakerService _tournamentTieBreakerService;

        public TournamentTieBreakerController(TournamentTieBreakerService tournamentTieBreakerService)
        {
            _tournamentTieBreakerService = tournamentTieBreakerService;
        }

        // GET: api/TournamentTieBreaker
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<TournamentTieBreakerDto>>> getTournamentTieBreakersAsync()
        {
            try
            {
                IEnumerable<TournamentTieBreakerDto> tiebreakers = await _tournamentTieBreakerService.getTournamentTieBreakersAsync();   

                return Ok(new ResponseDto { Success = true, Message = "Dados encontrados", Data = tiebreakers });
            }
            catch (Exception ex) 
            {
                return BadRequest(new ResponseDto { Success = false, Message = ex.Message });  
            }
        }
    }
}
