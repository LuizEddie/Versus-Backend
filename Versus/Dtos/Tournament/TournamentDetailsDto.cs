using Versus.Dtos.User;
using Versus.Models.Tournament;

namespace Versus.Dtos.Tournament
{
    public class TournamentDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public List<PlayerDetailsDto> PlayerDetails { get; set; }
        public TournamentSpecsDetailsDto TournamentSpecs { get; set; }
        public TournamentStatus TournamentStatus { get; set; }
        public UserDetailsDto UserDetails { get; set; }
        public List<MatchDetailsDto> MatchDetails { get; set; }
        public PlayerDetailsDto WinnerDetails { get; set; }
    }
}
