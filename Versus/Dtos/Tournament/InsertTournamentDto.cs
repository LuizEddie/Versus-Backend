using System.ComponentModel.DataAnnotations;
using Versus.Models.Tournament;

namespace Versus.Dtos.Tournament
{
    public class InsertTournamentDto
    {
        [Required]
        public string Name { get; set; }
        public List<InsertPlayerDto>? Players { get; set; }
        public InsertTournamentSpecDto TournamentSpec { get; set; }

    }
}
