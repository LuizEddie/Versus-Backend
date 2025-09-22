using System.ComponentModel.DataAnnotations;

namespace Versus.Dtos.Tournament
{
    public class InsertTournamentSpecDto
    {
        public int? PointsPerWin { get; set; }
        public int? PointsPerDraw { get; set; }
        public int? PointsPerLose { get; set; }
        [Required]
        public int WinConditionId { get; set; }
        [Required]
        public int? TypeId { get; set; }
        public TimeSpan? MatchDuration { get; set; }
        public int? QtyRounds { get; set; }
        public int? TieBreakerId { get; set; }
        public int? QtyGroups { get; set; }
    }
}
