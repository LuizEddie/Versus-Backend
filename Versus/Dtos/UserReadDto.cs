using System.ComponentModel.DataAnnotations;

namespace Versus.Dtos
{
    public class UserReadDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
