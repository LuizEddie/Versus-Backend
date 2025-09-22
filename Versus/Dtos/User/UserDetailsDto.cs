using System.ComponentModel.DataAnnotations;

namespace Versus.Dtos.User
{
    public class UserDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhotoPath { get; set; } = string.Empty;

    }
}
