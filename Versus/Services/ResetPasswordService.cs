using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;
using Versus.Data;
using Versus.Dtos;
using Versus.Dtos.User;
using Versus.Models;
using Versus.Models.TemplateModels;
using Versus.Services.Exceptions;
using Versus.Settings;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Versus.Services
{
    public class ResetPasswordService
    {
        private readonly VersusContext _context;
        private readonly EmailService _emailService;
        private readonly TokenSettings _tokenSettings;
        private readonly EmailTemplateService _templateService;
        public ResetPasswordService(VersusContext context, EmailService emailService, IOptions<TokenSettings> tokenSettings, EmailTemplateService templateService)
        {
            _context = context;
            _emailService = emailService;
            _tokenSettings = tokenSettings.Value;
            _templateService = templateService;
        }

        public async Task SendNewPasswordEmailAsync(UserCreateDto user)
        {
            string userEmail = user.Email;
            string token = await GenerateToken(userEmail);
            string resetLink = $"http://localhost:4200/auth/confirm-register?token={token}";

            var body = await _templateService.RenderTemplateAsync("NewPassword.cshtml", new NewPasswordTemplateModel { ResetLink = resetLink, UserName = user.Name });

            await _emailService.SendMailAsync(userEmail, "Nova Senha", body);
            await InsertTokenAsync(token, userEmail);
        }

        public async Task SendRecoverPasswordEmailAsync(UserCreateDto user)
        {
            string userEmail = user.Email;
            string token = await GenerateToken(userEmail);
            string resetLink = $"http://localhost:4200/auth/confirm-new-password?token={token}";

            var body = await _templateService.RenderTemplateAsync("RecoverPassword.cshtml", new NewPasswordTemplateModel { ResetLink = resetLink, UserName = user.Name });

            await _emailService.SendMailAsync(userEmail, "Recuperação de Senha", body);
            await InsertTokenAsync(token, userEmail);
        }

        public async Task<string> GenerateToken(string userEmail)
        {
            try
            {
                await MarkTokensAsUsedByEmailAsync(userEmail);
                DateTime expiration = DateTime.UtcNow.AddMinutes(45);
                string data = $"{userEmail}|{expiration.Ticks}";

                using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_tokenSettings.SecretKey)))
                {
                    byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                    string hashBase64 = Convert.ToBase64String(hash);

                    string token = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{data}|{hashBase64}"));

                    return token;
                }
            }
            catch(DbUpdateException e)
            {
                throw new DbUpdateException(e.Message);
            }
           
        }

        public async Task<TokenValidatedDto> ValidateTokenAsync(string token)
        {
            try
            {
                string payload = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(token));

                var parts = payload.Split('|');
                //Valida composição do token
                if (parts.Length != 3) throw new TokenException("Token Inválido! Por favor clique em recuperar senha e tente novamente!");

                string email = parts[0];
                //Valida se o tick do token está ok
                if (!long.TryParse(parts[1], out long ticks)) throw new TokenException("Token Inválido! Por favor clique em recuperar senha e tente novamente!");

                DateTime expiresAt = new DateTime(ticks, DateTimeKind.Utc);
                //Valida se o token não expirou
                if (expiresAt < DateTime.UtcNow) throw new TokenException("Token Expirado! Por favor clique em recuperar senha e tente novamente!");

                //Valida se o hash do token é legítimo
                string recoveredHash = parts[2];

                using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_tokenSettings.SecretKey)))
                {
                    byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes($"{parts[0]}|{parts[1]}"));
                    string hashBase64 = Convert.ToBase64String(hash);

                    if (hashBase64 != recoveredHash) throw new TokenException("Token inválido! Por favor clique em recuperar senha e tente novamente!");
                }
                //Valida se o token existe no banco
                var tokenFromDb = await GetValidTokenByEmailAsync(email) ?? throw new TokenException("Token inválido! Por favor clique em recuperar senha e tente novamente!");
                //Valida se o token encontrado no banco é o mesmo que está sendo validado
                bool isTokenValid = BCrypt.Net.BCrypt.Verify(token, tokenFromDb.Token);

                if (!isTokenValid) throw new TokenException("Token Expirado! Por favor clique em recuperar senha e tente novamente!");

                return new TokenValidatedDto { Email = email, Token = token};
            }
            catch (FormatException)
            {
                throw new TokenException("Token inválido! Por favor clique em recuperar senha e tente novamente!");
            }
        }

        public async Task InsertTokenAsync(string token, string email)
        {
            string tokenBcrypt = BCrypt.Net.BCrypt.HashPassword(token);

            ResetPassword rs = new ResetPassword()
            {
                Email = email,
                Token = tokenBcrypt
            };

            _context.ResetPassword.Add(rs);

            await _context.SaveChangesAsync();
        }

        public async Task MarkTokensAsUsedByEmailAsync(string email)
        {
            try
            {
                await _context.ResetPassword.Where(x => x.Email == email).ExecuteUpdateAsync(x1 => x1.SetProperty(x2 => x2.Used, true));
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbUpdateException(e.Message);
            }
        }

        private async Task<ResetPassword> GetValidTokenByEmailAsync(string email)
        {
            return await _context.ResetPassword.FirstOrDefaultAsync(x => x.Email == email && x.Used == false);
        }

    }
}
