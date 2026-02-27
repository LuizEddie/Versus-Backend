using Humanizer;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using Versus.Data;
using Versus.Dtos;
using Versus.Dtos.User;
using Versus.Models;
using Versus.Models.Tournament;
using Versus.Services.Exceptions;

namespace Versus.Services
{
    public class UserService
    {
        private readonly VersusContext _context;
        private readonly AuthService _authService;
        private readonly IWebHostEnvironment _env;
        const long MAX_FILE_SIZE = 3 * 1024 * 1024;
        public UserService(VersusContext context, AuthService authService, IConfiguration config, IWebHostEnvironment env)
        {
            _context = context;
            _authService = authService;
            _env = env;
        }

        public async Task<UserDetailsDto> GetUserDataAsync(int Id)
        {
            User user = await _context.User.FirstOrDefaultAsync(x => x.Id == Id);

            if (user == null) return null;

            return new UserDetailsDto() { Email = user.Email, Id = user.Id, Name = user.Name, PhotoPath = user.PhotoPath };

        }

        public async Task InsertAsync(UserCreateDto dto)
        {
            bool userExists = await _context.User.AnyAsync(u => u.Email == dto.Email && u.IsDeleted == false);

            if (userExists) throw new InvalidOperationException("Este usuário já está cadastrado!");

            User user = new User()
            {
                Name = dto.Name,
                Email = dto.Email
            };

            _context.User.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UserUpdateDto user, int id)
        {
            User userData = await _context.User.FirstOrDefaultAsync(x => x.Id == id) ?? throw new NotFoundException("Usuário não encontrado");

            try
            {   
                if(user.Name != userData.Name && !string.IsNullOrEmpty(user.Name)) userData.Name = user.Name;
                if(user.IsDeleted.HasValue) userData.IsDeleted = user.IsDeleted.Value;
                if(user.IsDeleted.HasValue && user.IsDeleted.Value == true) userData.DeletedAt = DateTime.UtcNow;
                if(user.IsActive.HasValue) userData.IsActive = user.IsActive.Value;
                userData.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }

        public async Task UpdatePasswordAsync(UserUpdatePasswordDto dto)
        {
            try
            {
                var user = await GetUserByEmailAsync(dto.Email);

                user.UpdatedAt = DateTime.UtcNow;
                user.Email = dto.Email;
                user.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
                user.IsActive = true;

                _context.User.Update(user);
                await _context.SaveChangesAsync();

                await _authService.RevokeAllRefreshTokenByUserIdAsync(user.Id);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new DbConcurrencyException(ex.Message);
            }
        }

        public async Task UpdatePasswordByIdAsync(UserReceivePasswordDto dto, int id)
        {
            User user = await _context.User.FirstOrDefaultAsync(x => x.Id == id) ?? throw new NotFoundException("Usuário não encontrado");

            try
            {
                user.UpdatedAt = DateTime.UtcNow;
                user.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
                user.IsActive = true;

                await _context.SaveChangesAsync();

                await _authService.RevokeAllRefreshTokenByUserIdAsync(user.Id);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new DbConcurrencyException(ex.Message);
            }
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.User.Where(e => e.Email == email && e.IsDeleted == false)
                                           .OrderByDescending(e => e.Id)
                                           .FirstOrDefaultAsync() ?? throw new NotFoundException("Usuário ou senha incorretos. Caso tenha se esquecido das suas credenciais, por favor clique em Esqueci a Senha!");
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

            var result = await query.Select(u => new UserDetailsDto { Email = u.Email, Id = u.Id, Name = u.Name, PhotoPath = u.PhotoPath }).OrderByDescending(u => u.Id).ToListAsync();

            return result;
        }

        public async Task<UserDetailsDto> LoginAsync(LoginDto dto)
        {
            var user = await GetUserByEmailAsync(dto.Email);

            if (user.IsActive == false) throw new NotFoundException("Usuário ou senha incorretos. Caso tenha se esquecido das suas credenciais, por favor clique em Esqueci a Senha!");

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);

            if (!isPasswordValid) throw new NotFoundException("Usuário ou senha incorretos. Caso tenha se esquecido das suas credenciais, por favor clique em Esqueci a Senha!");

            return new UserDetailsDto { Email = user.Email, Name = user.Name, Id = user.Id, PhotoPath = user.PhotoPath };
        }

        public async Task<string> UploadProfileImageAsync(int id, IFormFile file)
        {
            User user = await _context.User.FirstOrDefaultAsync(x => x.Id == id) ?? throw new NotFoundException("Usuário não encontrado");
            
            var oldPhotoPath = user.PhotoPath;
            
            if (file == null || file.Length == 0) throw new UserException("Arquivo inválido");

            if (file.Length > MAX_FILE_SIZE) throw new UserException("A imagem deve ter no máximo 3MB");

            if (!file.ContentType.StartsWith("image/")) throw new UserException("Somente imagens são permitidas");

            var uploadPath = Path.Combine(_env.WebRootPath,"uploads", "profile");

            if(!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadPath, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            //var imgUrl = $"{_baseUrl}/uploads/profile/{fileName}";
            var imgUrl = Path.Combine("uploads", "profile", fileName).Replace("\\", "/");

            try
            {
                user.PhotoPath = imgUrl;
                await _context.SaveChangesAsync();

                DeleteOldImage(oldPhotoPath);

                return imgUrl;
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }

        }

        private void DeleteOldImage(string? path)
        {
            if (string.IsNullOrEmpty(path)) return;

            var fullPath = Path.Combine(_env.WebRootPath, path.Replace("/", Path.DirectorySeparatorChar.ToString()));

            if(File.Exists(fullPath)) File.Delete(fullPath);
        }

    }
}
