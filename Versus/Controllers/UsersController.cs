using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Versus.Data;
using Versus.Dtos;
using Versus.Dtos.User;
using Versus.Extensions;
using Versus.Models;
using Versus.Services;

namespace Versus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly ResetPasswordService _resetPasswordService;

        public UsersController(UserService userService, ResetPasswordService resetPasswordService)
        {
            _userService = userService;
            _resetPasswordService = resetPasswordService;
        }

        // GET: api/Users
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserDetailsDto>>> GetUsers()
        {
            var users = await _userService.GetUsersByParamsAsync(new UserFilterDto()
            {
                IsDeleted = false,
                IsActive = true,
            });

            if(users == null || !users.Any()) return NotFound(new ResponseDto{ Success = false, Message = "Usuários não encontrados" });
            
            return Ok(new ResponseDto{ Success = true, Message = "Usuários encontrados", Data = users });
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<UserDetailsDto>> GetUser(int id)
        {
            var user = await _userService.GetUserDataAsync(id);

            if (user == null) return NotFound(new ResponseDto { Success = false, Message = "Usuário não encontrado!"});

            user.PhotoPath = string.IsNullOrEmpty(user.PhotoPath) ? null : $"{Request.Scheme}://{Request.Host}/{user.PhotoPath}";

            return Ok(new ResponseDto { Success = true, Message = "Usuário encontrado", Data = user});
        }

        // GET: api/Users/Me
        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<UserDetailsDto>> GetUserByAccessTokenId()
        {
            try
            {
                var userId = User.GetUserId();
                var user = await _userService.GetUserDataAsync(userId);

                if (user == null) return NotFound(new ResponseDto { Success = false, Message = "Usuário não encontrado!" });

                user.PhotoPath = string.IsNullOrEmpty(user.PhotoPath) ? null : $"{Request.Scheme}://{Request.Host}/{user.PhotoPath}";

                return Ok(new ResponseDto { Success = true, Message = "Usuário encontrado", Data = user });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto { Success = false, Message = ex.Message });
            }
        }
        // PATCH: api/Users/5
        [HttpPatch("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDto user, int id)
        {
            try
            {
                await _userService.UpdateAsync(user, id);
                return Ok(new ResponseDto { Success = true, Message = "Usuário atualizado com sucesso" });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto { Success = false, Message = ex.Message });
            }
        }

        // PATCH: api/User/me/update-profile-picture
        [HttpPatch("me/update-profile-picture")]
        [Authorize]
        public async Task<IActionResult> UpdateProfilePicture(IFormFile file)
        {
            try
            {
                var userId = User.GetUserId();

                string url = await _userService.UploadProfileImageAsync(userId, file);

                string finalUrl = $"{Request.Scheme}://{Request.Host}/{url}";

                return Ok(new ResponseDto { Success = true, Message = "Foto atualizada com sucesso!", Data = new { profileUrl = finalUrl } });
            }
            catch (Exception ex) 
            { 
                return BadRequest(new ResponseDto { Success=false, Message = ex.Message });
            }
        }

        // PATCH: api/Users/me/update-user-data
        [HttpPatch("me/update-user-data")]
        [Authorize]
        public async Task<IActionResult> UpdateUserData([FromBody] UserUpdateDto user)
        {
            try
            {
                var userId = User.GetUserId();

                await _userService.UpdateAsync(user, userId);
                return Ok(new ResponseDto { Success = true, Message = "Usuário atualizado com sucesso" });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto { Success = false, Message = ex.Message });
            }
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPost]
        public async Task<IActionResult> PostUser([FromBody] UserCreateDto dto)
        {
            try
            {
                await _userService.InsertAsync(dto);
                await _resetPasswordService.SendNewPasswordEmailAsync(dto);

                return Created(string.Empty, new ResponseDto { Success = true, Message = "Usuário criado com sucesso! Um email para a definição de senha e confirmação do cadastro será enviado, verifique a sua caixa de entrada ou spam!" });
            }
            catch (Exception ex) { 
                return BadRequest(new ResponseDto { Success = false, Message = ex.Message});
            }
        }

        // DELETE: api/Users/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteUser(int id)
        //{
        //    var user = await _context.User.FindAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.User.Remove(user);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}
    }
}
