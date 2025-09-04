namespace Versus.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Password { get; set; } = string.Empty;
        public string? PhotoPath { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public int IsDeleted { get; set; } = 0;
        public DateTime? DeletedAt { get; set; }
        public User(){}

        public User(int id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }

    }
}
