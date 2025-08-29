namespace Versus.Models
{
    public class ResetPassword
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public bool Used { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
