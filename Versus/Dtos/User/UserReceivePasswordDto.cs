using System.ComponentModel.DataAnnotations;

namespace Versus.Dtos.User
{
    public class UserReceivePasswordDto
    {
        [Required]
        [MinLength(8)]
        public string Password { get; set; } = string.Empty;
        [Required]
        [MinLength(8)]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
