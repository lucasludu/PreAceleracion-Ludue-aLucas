using System.ComponentModel.DataAnnotations;

namespace Disney.Dtos.Genre
{
    public class GenreCreateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public IFormFile Image { get; set; }
    }
}
