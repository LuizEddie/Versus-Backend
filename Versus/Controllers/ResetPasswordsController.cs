using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Versus.Data;
using Versus.Dtos;
using Versus.Services;
using Versus.Services.Exceptions;

namespace Versus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResetPasswordsController : ControllerBase
    {
        private readonly ResetPasswordService _resetPasswordService;
        private readonly UserService _userService;
        public ResetPasswordsController(ResetPasswordService resetPasswordService, UserService userService)
        {
            _resetPasswordService = resetPasswordService;
            _userService = userService;
        }

        //PATCH: api/ResetPasswords/validate-token
        [HttpPatch("validate-token/{token}")]
        public async Task<ActionResult<bool>> ValidateToken(string token)
        {
            try
            {
                var validated = await _resetPasswordService.ValidateTokenAsync(token);
                return Ok(new ResponseDto { Success = true, Message = "Token validado com sucesso!", Data = validated });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto { Success = false, Message = ex.Message });
            }
            
        }

        // PATCH: api/ResetPasswords/update-password
        [HttpPatch("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UserReceivePasswordWithTokenDto dto) //Fazer um dto aqui
        {
            try
            {
                var validated = await _resetPasswordService.ValidateTokenAsync(dto.Token);
                //Continuar com o método de atualizar a senha
                if (dto.Password != dto.ConfirmPassword) throw new UserException("As senhas não são iguais!");

                UserUpdatePasswordDto userUpdatePasswordDto = new UserUpdatePasswordDto { Email = validated.Email, Password = dto.Password};

                await _userService.UpdatePasswordAsync(userUpdatePasswordDto);

                await _resetPasswordService.MarkTokensAsUsedByEmailAsync(validated.Email);

                return Ok(new ResponseDto { Success = true, Message = "Senha atualizada com sucesso!"});
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto { Success = false, Message = ex.Message });
            }
        }
    }
}
