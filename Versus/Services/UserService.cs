using Humanizer;
using Microsoft.EntityFrameworkCore;
using Versus.Data;
using Versus.Dtos.User;
using Versus.Models;
using Versus.Services.Exceptions;

namespace Versus.Services
{
    public class UserService
    {
        private readonly VersusContext _context;

        public UserService(VersusContext context)
        {
            _context = context;
        }

        public async Task<UserDetailsDto> GetUserDataAsync(int Id)
        {
            User user = await _context.User.FirstOrDefaultAsync(x => x.Id == Id);

            if (user == null) return null;

            return new UserDetailsDto() { Email = user.Email, Id = user.Id, Name = user.Name };

        }

        //Fazer um userInsertDto
        public async Task InsertAsync(UserCreateDto dto)
        {
            bool userExists = await _context.User.AnyAsync(u => u.Email == dto.Email && u.IsDeleted == 0);

            if (userExists) throw new InvalidOperationException("Este usuário já está cadastrado!");

            User user = new User()
            {
                Name = dto.Name,
                Email = dto.Email
            };

            _context.User.Add(user);
            await _context.SaveChangesAsync();
        }

        //public async Task UpdateAsync(User user)
        //{
        //    if(!await _context.User.AnyAsync(e => e.Id == user.Id)) throw new NotFoundException("Usuário não encontrado");

        //    try
        //    {
        //        user.UpdatedAt = DateTime.UtcNow;
        //        _context.Update(user);
        //        await _context.SaveChangesAsync();
        //    }
        //    catch(DbUpdateConcurrencyException e)
        //    {
        //        throw new DbConcurrencyException(e.Message);
        //    }
        //}

        public async Task UpdatePasswordAsync(UserUpdatePasswordDto dto)
        {
            try
            {
                var user = await GetUserByEmailAsync(dto.Email);

                user.UpdatedAt = DateTime.UtcNow;
                user.Email = dto.Email;
                user.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
                user.IsActive = 1;
                _context.User.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new DbConcurrencyException(ex.Message);
            }
            catch(NotFoundException ex)
            {
                throw new NotFoundException(ex.Message);
            }
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.User.Where(e => e.Email == email && e.IsDeleted == 0)
                                           .OrderByDescending(e => e.Id)
                                           .FirstOrDefaultAsync() ?? throw new NotFoundException("Usuário não encontrado");
        }

        public async Task<IEnumerable<UserDetailsDto>> GetUsersByParamsAsync(UserFilterDto dto)
        {
            var query = _context.User.AsQueryable();

            if (dto.Id.HasValue)
            {
                query = query.Where(u => u.Id == dto.Id);
            }

            if (!string.IsNullOrWhiteSpace(dto.Email))
            {
                query = query.Where(u => u.Email == dto.Email);
            }

            if (dto.IsDeleted.HasValue)
            {
                query = query.Where(u => u.IsDeleted == dto.IsDeleted);
            }

            if (dto.IsActive.HasValue)
            {
                query = query.Where(u => u.IsActive == dto.IsActive);
            }

            var result = await query.Select(u => new UserDetailsDto { Email = u.Email, Id = u.Id, Name = u.Name }).OrderByDescending(u => u.Id).ToListAsync();

            return result;
        }

    }
}
