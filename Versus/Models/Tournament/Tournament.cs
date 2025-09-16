namespace Versus.Models.Tournament
{
    public class Tournament
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? FinishedAt {  get; set; }
        public int StatusId { get; set; } = 1;
        public TournamentStatus Status { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<Player> Players { get; set; } = new List<Player>();
        public TournamentSpec TournamentSpec { get; set; }
    }
}
