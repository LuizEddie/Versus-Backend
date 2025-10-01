using Versus.Models.Tournament;

namespace Versus.Dtos.Tournament
{
    public class MatchDetailsDto
    {
        public int Id { get; set; }
        public string? Phase { get; set; }
        public string? Round { get; set; }
        public DateTime MatchDate { get; set; }
        public PlayerDetailsDto? Player1 { get; set; }
        public PlayerDetailsDto? Player2 { get; set; }
        public PlayerDetailsDto? Winner { get; set; }
        public int? Group { get; set; }
        public string? Key  { get; set; }
    }
}
