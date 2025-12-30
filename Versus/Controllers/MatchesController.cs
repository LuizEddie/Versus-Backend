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
        [HttpPost("create-first-knockout-stage-matches/{tournamentId}")]
        [Authorize]
        public async Task<IActionResult> CreateFirstKnockoutMatches(int tournamentId)
        {
            try
            {
                await _matchService.CreateFirstKnockoutStageMatchesAsync(tournamentId);
                return Ok(new ResponseDto { Success = true, Message = "Primeira rodada do mata-mata criada com sucesso!" });
            }
            catch(Exception ex)
            {
                return BadRequest(new ResponseDto { Success = false, Message = ex.Message });
            }

        }

        //GET: api/Matches/FindSingleMatch
        [HttpGet("find-single-match")]
        [Authorize]
        public async Task<IActionResult> FindSingleMatch([FromQuery] MatchFilterDto dto)
        {
            try
            {
                MatchDetailsDto match = await _matchService.FindSingleMatchAsync(dto);
                if (match == null) return BadRequest(new ResponseDto { Success = false, Message = "Partida não encontrada" });
                return Ok(new ResponseDto { Success = true, Message = "Partida encontrada!", Data = match });
            }
            catch (Exception ex) {
                return BadRequest(new ResponseDto { Success = false, Message = ex.Message });
            }
        }

        //PATCH: api/Matches/5
        [HttpPatch("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateMatch([FromBody] UpdateMatchDto dto, int id)
        {
            try
            {
                await _matchService.UpdateMatchAsync(id, dto);
                return Ok(new ResponseDto { Success = true, Message = "Partida atualizada!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto { Success = false, Message = ex.Message });

            }
        }
    }
}
