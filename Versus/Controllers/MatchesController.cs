using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Versus.Dtos.Tournament;
using Versus.Dtos;
using Microsoft.AspNetCore.Authorization;
using Versus.Services;

namespace Versus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchesController : ControllerBase
    {
        private readonly MatchService _matchService;

        public MatchesController(MatchService matchService)
        {
            _matchService = matchService;
        }

        //POST: api/Matches
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> InsertMatch([FromBody] InsertMatchDto dto)
        {
            try
            {
                await _matchService.InsertMatchAsync(dto);
                return Created(string.Empty, new ResponseDto { Success = true, Message = "Partida adicionada com sucesso!" });
            }
            catch (Exception ex) 
            {
                return BadRequest(new ResponseDto { Success = false, Message = ex.Message });
            }
        }

        //POST: api/Matches/CreateFirstKnockoutStageMatches/5
        [HttpPost("CreateFirstKnockoutStageMatches/{tournamentId}")]
        [Authorize]
        public async Task<IActionResult> CreateFirstKnockoutMatches(int tournamentId)
        {
            await _matchService.CreateFirstKnockoutStageMatchesAsync(tournamentId);
            return Ok(new ResponseDto { Success = true, Message = "Teste"});
        }
    }
}
