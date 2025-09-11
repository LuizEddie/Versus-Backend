namespace Versus.Models.Tournament
{
    public class Classification
    {
        public int Id { get; set; }
        public int Points { get; set; } = 0;
        public int Wins { get; set; } = 0;
        public int Loses { get; set; } = 0;
        public int Draws { get; set; } = 0;
        public int TournamentId { get; set; }
        public Tournament Tournament { get; set; }
        public int PlayerId { get; set; }
        public Player Player { get; set; }
    }
}
