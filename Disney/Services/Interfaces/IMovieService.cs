using Disney.Dtos.Movies;
using Disney.Utilities;

namespace Disney.Services.Interfaces
{
    public interface IMovieService
    {
        Task<MovieDto> AddMovie(MovieCreateDto movieCreateDto);
        Task<IList<MovieListDto>> GetAllMovies();
        Task<MovieDetailsDto> GetMovieDetails(int id);
        Task AddCharacterToMovie(int movieId, int characterId);
        Task<IList<MovieListDto>> GetMoviesByTitle(string title);
        Task<IList<MovieListDto>> GetMoviesByGenre(int genreId);
        Task<IList<MovieListDto>> GetMoviesByReleased(Order order);
        Task<MovieCreateDto> UpdateMovie(int id, MovieCreateDto movieUpdate);

    }
}
