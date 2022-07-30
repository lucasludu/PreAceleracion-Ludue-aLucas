using Disney.Entities;

namespace Disney.Dtos.Character
{
    public class CharacterGetDto
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int Weight { get; set; }
        public string History { get; set; }
        public IList<Movie> Movies { get; set; }
    }
}
