using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace Versus.Models.Tournament
{
    public class Match
    {
        public int Id { get; set; }
        public string? Round { get; set; } //-- útil para mata-mata, pode ficar nulo em pontos corridos
        public DateTime MatchDate { get; set; } = DateTime.UtcNow;
        public int TournamentId { get; set; }
        public Tournament Tournament { get; set; }
        public int? Player1Id { get; set; }
        public Player? Player1 { get; set; }
        public int? Player2Id { get; set; }
        public Player? Player2 { get; set; }
        public int? WinnerId { get; set; }
        public Player? Winner { get; set; }
        public string? Key { get; set; }
    }
}
