using Versus.Models.Tournament;

namespace Versus.Dtos.Tournament
{
    public class TournamentSpecsDetailsDto
    {
        public int Id { get; set; }
        public int PointsPerWin { get; set; }
        public int PointsPerDraw { get; set; }
        public int PointsPerLose { get; set; }
        public string WinCondition { get; set; }
        public TimeSpan MatchDuration { get; set; }
        public int QtyRounds { get; set; }
        public string TieBreaker { get; set; }
        public TournamentType Type { get; set; }
    }
}
