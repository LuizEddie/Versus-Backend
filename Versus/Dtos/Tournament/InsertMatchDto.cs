using Versus.Models.Tournament;

namespace Versus.Dtos.Tournament
{
    public class InsertMatchDto
    {
        public string? Phase { get; set; }
        public string? Round { get; set; }
        public int TournamentId { get; set; }
        public int Player1Id { get; set; }
        public int? Player2Id { get; set; }
        public int? Group { get; set; }
        public string? Key { get; set; }
    }
}
