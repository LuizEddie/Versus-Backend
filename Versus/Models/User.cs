namespace Versus.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty ;
        public string PhotoPath { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public User(){}

        public User(int id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }

    }
}
