using System.ComponentModel.DataAnnotations;

namespace Disney.Entities
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }
        public string Image{ get; set; }
        [Required]
        public string Name { get; set; }

        public IList<Movie> Movies { get; set; }
    }
}
