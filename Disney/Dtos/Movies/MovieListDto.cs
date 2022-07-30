using System.Text.Json.Serialization;

namespace Disney.Dtos.Movies
{
    public class MovieListDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public int Released { get; set; }

        
    }
}
