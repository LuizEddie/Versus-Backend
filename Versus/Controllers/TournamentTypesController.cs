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
    public class TournamentTypesController : ControllerBase
    {
        private readonly TournamentTypeService _tournamentTypeService;

        public TournamentTypesController(TournamentTypeService tournamentTypeService)
        {
            _tournamentTypeService = tournamentTypeService;
        }

        // GET: api/TournamentTypes
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<TournamentTypeDto>>> getTournamentTypes()
        {
            try
            {
                IEnumerable<TournamentTypeDto> tournamentTypes = await _tournamentTypeService.getTournamentTypesAsync();
                return Ok(new ResponseDto { Success = true, Message = "Dados carregados com sucesso", Data = tournamentTypes });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto { Success = false, Message = ex.Message });
            }
        }
    }
}
