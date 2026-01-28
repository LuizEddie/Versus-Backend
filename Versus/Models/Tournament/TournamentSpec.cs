namespace Versus.Models.Tournament
{
    public class TournamentSpec
    {
        public int Id { get; set; }
        public int? WinConditionId { get; set; }
        public TournamentWinConditon? WinCondition { get; set; }
        public int TournamentId { get; set; }
        public Tournament Tournament { get; set; }
        public int TypeId { get; set; }
        public TournamentType Type { get; set; }
    }
}
