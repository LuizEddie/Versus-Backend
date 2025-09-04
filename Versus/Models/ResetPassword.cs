namespace Versus.Models
{
    public class ResetPassword
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public int Used { get; set; } = 0;
        public string Token { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
