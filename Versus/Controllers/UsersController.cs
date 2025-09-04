using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Versus.Data;
using Versus.Dtos;
using Versus.Models;
using Versus.Services;

namespace Versus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly VersusContext _context;
        private readonly UserService _userService;
        private readonly ResetPasswordService _resetPasswordService;

        public UsersController(VersusContext context, UserService userService, ResetPasswordService resetPasswordService)
        {
            _context = context;
            _userService = userService;
            _resetPasswordService = resetPasswordService;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return await _context.User.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _userService.GetUserDataAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<User>> PostUser([FromBody] UserCreateDto dto)
        {
            try
            {
                User user = new User()
                {
                    Name = dto.Name,
                    Email = dto.Email
                };

                await _userService.InsertAsync(user);
                await _resetPasswordService.SendNewPasswordEmailAsync(user);

                UserReadDto userReadDto = new UserReadDto()
                {
                    Name = user.Name,
                    Email = user.Email
                };

                //return CreatedAtAction("GetUser", new { id = user.Id }, userReadDto);
                return Ok(new { success = true, message = "Usuário criado com sucesso! Um email para a definição de senha e confirmação do cadastro será enviado!" });
            }
            catch (Exception ex) { 
                return BadRequest(new { success = false, message = ex.Message});
            }
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
