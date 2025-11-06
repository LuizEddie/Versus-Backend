using Versus.Models.Tournament;

namespace Versus.Dtos.Tournament
{
    public class MatchFilterDto
    {
        public int? Id { get; set; }
        public string? Phase { get; set; } //-- útil para pontos corridos
        public string? Round { get; set; } //-- útil para mata-mata, pode ficar nulo em pontos corridos
        public DateTime? MatchDate { get; set; }
        public int? TournamentId { get; set; }
        public int? Player1Id { get; set; }
        public int? Player2Id { get; set; }
        public int? WinnerId { get; set; }
        public int? Group { get; set; }
        public string? Key { get; set; }
    }
}
