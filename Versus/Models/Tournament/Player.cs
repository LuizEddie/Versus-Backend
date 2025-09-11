namespace Versus.Models.Tournament
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TournamentId { get; set; }
        public Tournament Tournament { get; set; }
    }
}
