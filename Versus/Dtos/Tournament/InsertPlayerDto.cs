using System.ComponentModel.DataAnnotations;

namespace Versus.Dtos.Tournament
{
    public class InsertPlayerDto
    {
        [Required]
        public string Name { get; set; }
    }
}
