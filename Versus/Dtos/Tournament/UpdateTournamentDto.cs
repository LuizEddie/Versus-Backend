using System.ComponentModel.DataAnnotations;

namespace Versus.Dtos.Tournament
{
    public class UpdateTournamentDto
    {
        public string? Name { get; set; }
        public int? StatusId { get; set; }
        public int? WinnerId { get; set; }
    }
}
