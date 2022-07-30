using Disney.Dtos.Genre;
using Disney.Entities;

namespace Disney.Services.Interfaces
{
    public interface IGenreService
    {
        Task<GenreDto> AddCharacter(GenreCreateDto genreCreateDto);
        Task<IList<GenreDto>> GetAllGenres();
        Task AddMovieToGenre(int genreId, int movieId);
    }
}
