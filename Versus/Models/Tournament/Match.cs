namespace Versus.Models.Tournament
{
    public class Match
    {
        public int Id { get; set; }
        public string? Phase { get; set; }
        public string? Round { get; set; }
        public DateTime MatchDate { get; set; }
        public int TournamentId { get; set; }
        public Tournament Tournament { get; set; }
        public int? Player1Id { get; set; }
        public Player? Player1 { get; set; }
        public int? Player2Id { get; set; }
        public Player? Player2 { get; set; }
        public int? WinnerId { get; set; }
        public Player? Winner { get; set; }
        public string? WinCondition { get; set; }
    }
}
