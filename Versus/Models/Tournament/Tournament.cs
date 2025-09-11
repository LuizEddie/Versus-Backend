namespace Versus.Models.Tournament
{
    public class Tournament
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? FinishedAt {  get; set; }
        public int StatusId { get; set; }
        public TournamentStatus Status { get; set; }
        public int TypeId { get; set; }
        public TournamentType Type { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
