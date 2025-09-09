using System.ComponentModel.DataAnnotations;

namespace Versus.Dtos
{
    public class RequestRecoverPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
