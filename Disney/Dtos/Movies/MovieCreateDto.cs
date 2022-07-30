using System.ComponentModel.DataAnnotations;

namespace Disney.Dtos.Movies
{
    public class MovieCreateDto
    {
        private static int currentYear = DateTime.UtcNow.Year;
        private int _released;
        public IFormFile Image { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [Range(1888, 3000)]
        public int Released
        {
            get
            {
                if(_released > DateTime.UtcNow.Year)
                {
                    return DateTime.UtcNow.Year;
                }
                return _released;
            }
            set
            {
                _released = value;
            }
        }
        [Required]
        [Range(1, 5)]
        public int Rate { get; set; }
    }
}
