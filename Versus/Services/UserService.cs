using Microsoft.EntityFrameworkCore;
using Versus.Data;
using Versus.Models;
using Versus.Services.Exceptions;

namespace Versus.Services
{
    public class UserService
    {
        private readonly VersusContext _context;

        public UserService(VersusContext context) { 
            _context = context;
        }

        public async Task<User> GetUserDataAsync(int Id)
        {
            return await _context.User.FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task InsertAsync(User user)
        {
            bool userExists = await _context.User.AnyAsync(u => u.Email == user.Email && u.IsDeleted == 0);

            if (userExists) throw new InvalidOperationException("Este usuário já está cadastrado!");

            _context.User.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            if(!await UserExistsAsync(user.Id))
            {
                throw new NotFoundException("Usuário não encontrado");
            }

            try
            {
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }

        private async Task<bool> UserExistsAsync(int id)
        {
            return await _context.User.AnyAsync(e => e.Id == id);
        }

    }
}
