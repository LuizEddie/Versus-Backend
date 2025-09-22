using System.ComponentModel.DataAnnotations;

namespace Versus.Dtos.Tournament
{
    public class UpdatePlayerDto
    {
        [Required]
        public string Name { get; set; }
    }
}
