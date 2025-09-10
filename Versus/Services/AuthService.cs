using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Versus.Data;
using Versus.Models;
using Versus.Services.Exceptions;

namespace Versus.Services
{
    public class AuthService
    {
        private readonly IConfiguration _config;
        private readonly VersusContext _context;
        public AuthService(UserService userService, IConfiguration config, VersusContext context)
        {
            _config = config;
            _context = context;
        }
        public string GenerateJwtToken(int id)
        {
            var jwtSettings = _config.GetSection("Jwt");

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                    issuer: jwtSettings["Issuer"],
                    audience: jwtSettings["Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(15),
                    signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<RefreshToken> GenerateRefreshTokenAsync(int id)
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                UserId = id
            };

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return refreshToken;
        }

        public async Task RevokeAllRefreshTokenByUserIdAsync(int id)
        {
            try
            {
                await _context.RefreshTokens.Where(x => x.UserId == id).ExecuteUpdateAsync(x1 => x1.SetProperty(x2 => x2.IsRevoked, true));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new DbConcurrencyException(ex.Message);
            }
        }

        public async Task<RefreshToken> GetRefreshTokenAsync(string token)
        {
            var storedToken = await _context.RefreshTokens.Include(r => r.User).FirstOrDefaultAsync(r => r.Token == token);

            if (storedToken == null || storedToken.IsRevoked || storedToken.ExpiresAt < DateTime.UtcNow) return null;

            return storedToken;
        }
    }
}
