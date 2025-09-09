using System.ComponentModel.DataAnnotations;

namespace Versus.Dtos
{
    public class UserReceivePasswordWithTokenDto
    {
        [Required]
        public string Token { get; set; } = string.Empty;
        [Required]
        [MinLength(8)]
        public string Password { get; set; } = string.Empty;
        [Required]
        [MinLength(8)]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
