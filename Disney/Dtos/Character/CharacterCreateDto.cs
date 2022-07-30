using System.ComponentModel.DataAnnotations;

namespace Disney.Dtos.Character
{
    public class CharacterCreateDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public int Weight { get; set; }
        [Required]
        public string History { get; set; }
        public IFormFile Image { get; set; }
    }
}
