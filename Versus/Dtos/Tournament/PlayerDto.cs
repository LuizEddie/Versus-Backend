using System.ComponentModel.DataAnnotations;

namespace Versus.Dtos.Tournament
{
    public class PlayerDto
    {
        [Required]
        public string Name { get; set; }
    }
}
