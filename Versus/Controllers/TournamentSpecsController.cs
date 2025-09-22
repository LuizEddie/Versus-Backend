using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Versus.Dtos;
using Versus.Dtos.Tournament;
using Versus.Services;
using Versus.Services.Exceptions;

namespace Versus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentSpecsController : ControllerBase
    {
        private readonly TournamentSpecService _tournamentSpecService;

        public TournamentSpecsController(TournamentSpecService tournamentSpecService)
        {
            _tournamentSpecService = tournamentSpecService;
        }

        // PATCH: api/TournamentSpecs/5
        [HttpPatch("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateTournamentSpec([FromBody] UpdateTournamentSpecsDto dto, int id)
        {
            try
            {
                await _tournamentSpecService.UpdateSpecAsync(dto, id);
                return Ok(new ResponseDto { Success = true, Message = "Especificações atualizadas com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto { Success = false, Message = ex.Message});
            }
        }
    }
}
