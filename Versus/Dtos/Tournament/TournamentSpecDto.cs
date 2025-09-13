using System.ComponentModel.DataAnnotations;

namespace Versus.Dtos.Tournament
{
    public class TournamentSpecDto
    {
        public int? PointsPerWin { get; set; }
        public int? PointsPerDraw { get; set; }
        public int? PointsPerLose { get; set; }
        [Required]
        public string WinCondition { get; set; }
        [Required]
        public int? TypeId { get; set; }
        public TimeSpan? MatchDuration { get; set; }
        public int? QtyRounds { get; set; }
        public string? TieBreaker { get; set; }
    }
}
