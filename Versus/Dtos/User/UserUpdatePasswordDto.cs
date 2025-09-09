using System.ComponentModel.DataAnnotations;

namespace Versus.Dtos.User
{
    public class UserUpdatePasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; } = string.Empty;
    }
}
