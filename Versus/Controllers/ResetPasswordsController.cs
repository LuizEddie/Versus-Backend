using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Versus.Data;
using Versus.Services;
using Versus.Services.Exceptions;

namespace Versus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResetPasswordsController : ControllerBase
    {
        private readonly VersusContext _context;
        private readonly ResetPasswordService _resetPasswordService;

        public ResetPasswordsController(VersusContext context, ResetPasswordService resetPasswordService)
        {
            _context = context;
            _resetPasswordService = resetPasswordService;
        }

        //PATCH: api/ResetPasswords/validate-token
        [HttpPatch("validate-token/{token}")]
        public ActionResult<bool> ValidateToken(string token)
        {
            try
            {
                return _resetPasswordService.ValidateTokenAsync(token);
            }
            catch (Exception ex) {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
