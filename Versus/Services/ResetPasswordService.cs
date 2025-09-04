using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;
using Versus.Data;
using Versus.Models;
using Versus.Models.TemplateModels;
using Versus.Settings;

namespace Versus.Services
{
    public class ResetPasswordService
    {
        private readonly VersusContext _context;
        private readonly EmailService _emailService;
        private readonly TokenSettings _tokenSettings;
        private readonly EmailTemplateService _templateService;
        public ResetPasswordService(VersusContext context, EmailService emailService, IOptions<TokenSettings> tokenSettings, EmailTemplateService templateService) { 
            _context = context;
            _emailService = emailService;
            _tokenSettings = tokenSettings.Value;
            _templateService = templateService;
        }

        public async Task SendNewPasswordEmailAsync(User user)
        {
            string userEmail = user.Email;
            string token = GenerateToken(userEmail);
            string resetLink = $"https://localhost:3000/confirmar-cadastro/{token}";

            var body = await _templateService.RenderTemplateAsync("NewPassword.cshtml", new NewPasswordTemplateModel() { ResetLink = resetLink, UserName = user.Name }); 

            await _emailService.SendMailAsync(userEmail, "Nova Senha", body);
            await InsertTokenAsync(token, userEmail);
        }

        public string GenerateToken(string userEmail)
        {
            DateTime expiration = DateTime.UtcNow.AddMinutes(30);
            string data = $"{userEmail}|{expiration.Ticks}";

            using(var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_tokenSettings.SecretKey)))
            {
                byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                string hashBase64 = Convert.ToBase64String(hash);

                string token = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{data}|{hashBase64}"));

                return token;
            }
        }

        public bool ValidateToken()
        {
            return true;
        }

        public async Task InsertTokenAsync(string token, string email)
        {
            DateTime now = DateTime.UtcNow;

            now = new DateTime(now.Ticks - (now.Ticks % TimeSpan.TicksPerSecond));

            string tokenBcrypt = BCrypt.Net.BCrypt.HashPassword(token);

            ResetPassword rs = new ResetPassword()
            {
                Email = email,
                Token = tokenBcrypt
            };

            _context.ResetPassword.Add(rs);

            await _context.SaveChangesAsync();
        }
    }
}
