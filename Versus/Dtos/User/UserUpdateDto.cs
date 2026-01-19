using System.ComponentModel.DataAnnotations;

namespace Versus.Dtos.User
{
    public class UserUpdateDto
    {
        public string? Name { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }

    }
}
