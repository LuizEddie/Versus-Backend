namespace Versus.Models.Tournament
{
    public class TournamentSpec
    {
        public int Id { get; set; }
        public int? PointsPerWin { get; set; }
        public int? PointsPerDraw { get; set; }
        public int? PointsPerLose { get; set; }
        public int WinConditionId { get; set; }
        public TournamentWinConditon WinCondition { get; set; }
        public TimeSpan? MatchDuration { get; set; }
        public int? QtyRounds { get; set; }
        public int? TieBreakerId { get; set; }
        public TournamentTieBreaker? TieBreaker { get; set; }
        public int TournamentId { get; set; }
        public Tournament Tournament { get; set; }
        public int TypeId { get; set; }
        public TournamentType Type { get; set; }
        public int? QtyGroups { get; set; }
    }
}
