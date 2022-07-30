using Disney.Dtos.Character;

namespace Disney.Services.Interfaces
{
    public interface ICharacterService
    {
        Task<CharacterDto> AddCharacter(CharacterCreateDto characterCreateDto);
        Task<CharacterGetDto> GetById(int id);
        Task<IList<CharacterListItemDto>> GetAll();
        Task<CharacterListItemDto> GetByName(string name);
        Task<IList<CharacterListItemDto>> GetCharactersByAge(int age);
        Task<IList<CharacterListItemDto>> GetCharactersFromMovie(int movieId);
        Task<CharacterCreateDto> UpdateCharacter(int id, CharacterCreateDto characterUpdate);
    }
}
