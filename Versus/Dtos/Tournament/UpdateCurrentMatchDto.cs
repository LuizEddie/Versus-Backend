namespace Versus.Dtos.Tournament
{
    public class UpdateCurrentMatchDto
    {
        public string? Phase { get; set; }
        public string? Round { get; set; }
        public int? Group {  get; set; }

        public int Player2Id { get; set; }
    }
}
