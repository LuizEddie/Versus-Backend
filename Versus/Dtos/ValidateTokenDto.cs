using System.ComponentModel.DataAnnotations;

namespace Versus.Dtos
{
    public class ValidateTokenDto
    {
        [Required]
        public string Token { get; set; } = string.Empty;
    }
}
