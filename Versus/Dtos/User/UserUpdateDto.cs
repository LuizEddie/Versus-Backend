using System.ComponentModel.DataAnnotations;

namespace Versus.Dtos.User
{
    public class UserUpdateDto
    {
        [Required]
        public string Name { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsActive { get; set; }

    }
}
