using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Versus.Dtos;
using Versus.Dtos.User;
using Versus.Extensions;
using Versus.Models;
using Versus.Services;

namespace Versus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly AuthService _authService;
        
        public AuthController(UserService userService, AuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        // POST: api/Auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                UserDetailsDto user = await _userService.LoginAsync(dto);

                var refreshToken = await _authService.GenerateRefreshTokenAsync(user.Id);

                var accessToken = _authService.GenerateJwtToken(user.Id);

                Response.Cookies.Append("refreshToken", refreshToken.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = refreshToken.ExpiresAt
                });

                return Ok(new ResponseDto { Success = true, Message = "Login efetuado com sucesso", Data = accessToken });
            }
            catch (Exception ex) { 
                return Unauthorized(new ResponseDto { Success = true, Message = ex.Message});
            }
        }

        // POST: api/Auth/refresh
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            try
            {
                if (!Request.Cookies.TryGetValue("refreshToken", out var refreshTokenValue)) return Unauthorized(new ResponseDto { Success = false, Message = "Refresh Token Ausente" });

                var storedToken = await _authService.GetRefreshTokenAsync(refreshTokenValue);

                if (storedToken == null || storedToken.IsRevoked) return Unauthorized(new ResponseDto { Success = false, Message = "Refresh Token inválido ou expirado." });

                if (storedToken.ExpiresAt < DateTime.UtcNow)
                {
                    await _authService.RevokeRefreshTokenAsync(refreshTokenValue);
                    return Unauthorized(new ResponseDto { Success = false, Message = "Refresh Token inválido ou expirado." });
                }

                await _authService.RevokeRefreshTokenAsync(refreshTokenValue);

                var refreshToken = await _authService.GenerateRefreshTokenAsync(storedToken.UserId);
                
                var accessToken = _authService.GenerateJwtToken(storedToken.UserId);

                Response.Cookies.Append("refreshToken", refreshToken.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = refreshToken.ExpiresAt
                });

                return Ok(new ResponseDto { Success = true, Message = "Token atualizado com sucesso!", Data = accessToken });
            }
            catch (Exception ex) {
                return Unauthorized(new ResponseDto { Success = false, Message = ex.Message });
            }
        }

        //POST: api/Auth/logout
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                if(Request.Cookies.TryGetValue("refreshToken", out var refreshTokenValue))
                {
                    var storedToken = await _authService.GetRefreshTokenAsync(refreshTokenValue);

                    if (storedToken != null && !storedToken.IsRevoked)
                    {
                        await _authService.RevokeRefreshTokenAsync(refreshTokenValue);
                    }

                }

                Response.Cookies.Delete("refreshToken");

                return Ok(new ResponseDto { Success = true, Message = "Logout realizado com sucesso!"});

            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto { Success = false, Message = ex.Message });
            }
        }

        //POST: api/Auth/global-logout
        [HttpPost("global-logout")]
        public async Task<IActionResult> GlobalLogout()
        {
            try
            {
                var userId = User.GetUserId();

                await _authService.RevokeAllRefreshTokenByUserIdAsync(userId);

                Response.Cookies.Delete("refreshToken");

                return Ok(new ResponseDto { Success = true, Message = "Logout realizado com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto { Success = false, Message = ex.Message });
            }
        }
    }
}
