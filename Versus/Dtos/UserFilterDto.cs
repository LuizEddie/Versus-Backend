namespace Versus.Dtos
{
    public class UserFilterDto
    {
        public int? Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int? IsActive { get; set; }
    }
}
