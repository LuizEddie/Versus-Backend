namespace Versus.Models.Tournament
{
    public class TournamentSpec
    {
        public int Id { get; set; }
        public int PointsPerWin { get; set; }
        public int PointsPerDraw { get; set; }
        public int PointsPerLose { get; set; }
        public string WinCondition { get; set; }
        public string MatchDuration { get; set; }
        public int QtyRounds { get; set; }
        public string TieBreaker { get; set; }
        public int TournamentId { get; set; }
        public Tournament Tournament { get; set; }
    }
}
