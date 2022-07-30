using Disney.Dtos.Character;

namespace Disney.Dtos.Movies
{
    public class MovieDetailsDto
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Image { get; set; }

        public int Released { get; set; }

        public int Rate { get; set; }

      
        public IList<CharacterListItemDto> Characters { get; set; }
    }
}
