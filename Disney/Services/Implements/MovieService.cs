using AutoMapper;
using Disney.Data;
using Disney.Dtos.Character;
using Disney.Dtos.Movies;
using Disney.Entities;
using Disney.Services.Interfaces;
using Disney.UOWork;
using Disney.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Disney.Services.Implements
{
    public class MovieService : IMovieService
    {
        private readonly IUnitOfWork _uowork;
        private readonly IMapper _mapper;
        private readonly IFileStorage _fileStorage;
        private readonly ApplicationDbContext _context;

        public MovieService(IUnitOfWork uowork, IMapper mapper, IFileStorage fileStorage, ApplicationDbContext context)
        {
            _uowork = uowork;
            _mapper = mapper;
            _fileStorage = fileStorage;
            _context = context;
        }

        #region AddMovie
        public async Task<MovieDto> AddMovie (MovieCreateDto movieCreateDto)
        {
            if (movieCreateDto == null)
            {
                throw new ArgumentOutOfRangeException("No movie-create-dto found.");
            }

            var entity = _mapper.Map<Movie>(movieCreateDto);

            if (movieCreateDto.Image != null)
            {
                string imageUrl = await SaveImage(movieCreateDto.Image);
                entity.Image = imageUrl;
            }
            _uowork.MovieRepo.Insert(entity);
            _uowork.Save();

            var dto = _mapper.Map<MovieDto>(entity);
            return dto;
        }
        #endregion

        #region SaveImage
        private async Task<string> SaveImage(IFormFile image)
        {
            using var stream = new MemoryStream();
            await image.CopyToAsync(stream);

            var fileBytes = stream.ToArray();
            return await _fileStorage.Create(
                fileBytes,
                image.ContentType,
                Path.GetExtension(image.FileName),
                ConstantApplications.FileContainer.FileMovies,
                Guid.NewGuid().ToString()
            );
        }
        #endregion

        #region GetAll
        public async Task<IList<MovieListDto>> GetAllMovies()
        {
            var existingMovies = _uowork.MovieRepo.GetAll();
            var result = existingMovies.Select(m => _mapper.Map<MovieListDto>(m)).ToList();
            return result;
        }
        #endregion

        #region GetMovieDetails
        public async Task<MovieDetailsDto> GetMovieDetails(int id)
        {
            var existingMovie = await _context.Movies
                                    .Include(m => m.Characters)
                                    .FirstOrDefaultAsync(m => m.Id == id);

            if (existingMovie == null)
                throw new ArgumentOutOfRangeException("Movie not found.");

            var result = _mapper.Map<MovieDetailsDto>(existingMovie);
            result.Characters = existingMovie.Characters
                                    .Select(c => _mapper.Map<CharacterListItemDto>(c))
                                    .ToList();

            return result;
        }
        #endregion

        #region AddCharacterToMovie
        public async Task AddCharacterToMovie(int movieId, int characterId)
        {
            var existingMovie = await _context.Movies
                                    .Include(m => m.Characters)
                                    .FirstOrDefaultAsync(m => m.Id == movieId);

            if (existingMovie == null)
                throw new ArgumentOutOfRangeException("Movie not found.");

            var existingCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == characterId);

            if (existingCharacter == null)
                throw new ArgumentOutOfRangeException("Character not found.");

            existingMovie.Characters.Add(existingCharacter);

            await _context.SaveChangesAsync();
        }
        #endregion

        #region GetMoviesByTitle
        public async Task<IList<MovieListDto>> GetMoviesByTitle(string title)
        {
            var existingMovies = await _context.Movies
                                    .Where(m => m.Title.ToLower().Equals(title.ToLower()))
                                    .ToListAsync();

            if (existingMovies.Count == 0)
                return new MovieListDto[0];

            var result = existingMovies
                            .Select(m => _mapper.Map<MovieListDto>(m))
                            .ToList();

            return result;
        }
        #endregion

        #region GetMoviesByGenre
        public async Task<IList<MovieListDto>> GetMoviesByGenre(int genreId)
        {
            var existingGenre = await _context.Genres
                                .Include(g => g.Movies)
                                .FirstOrDefaultAsync(g => g.Id == genreId);

            if (existingGenre == null)
                throw new ArgumentOutOfRangeException("Genre not found.");

            var result = existingGenre.Movies
                            .Select(m => _mapper.Map<MovieListDto>(m))
                            .ToList();

            return result;
        }
        #endregion

        #region GetMoviesByReleased
        public async Task<IList<MovieListDto>> GetMoviesByReleased(Order order)
        {
            List<Movie> existingMovies = null;

            if (order == Order.DESC)
            {
                existingMovies = await _context.Movies
                                        .OrderByDescending(m => m.Released)
                                        .ToListAsync();
            }
            else
            {
                existingMovies = await _context.Movies
                                        .OrderBy(m => m.Released)
                                        .ToListAsync();
            }

            var result = existingMovies
                            .Select(m => _mapper.Map<MovieListDto>(m))
                            .ToList();

            return result;
        }
        #endregion

        #region Update
        public async Task<MovieCreateDto> UpdateMovie(int id, MovieCreateDto movieUpdate)
        {
            var entity = await _context.Movies.FirstOrDefaultAsync(a => a.Id == id);
            if (entity == null)
            {
                throw new ArgumentOutOfRangeException("No character-create-dto found.");
            }

            _mapper.Map(movieUpdate, entity);

            if (movieUpdate.Image != null)
            {
                if (!string.IsNullOrEmpty(entity.Image))
                {
                    await _fileStorage.Delete(entity.Image, ConstantApplications.FileContainer.FileMovies);
                }
                string pictureUrl = await SaveImage(movieUpdate.Image);
                entity.Image = pictureUrl;
            }
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChangesAsync();

            return movieUpdate;
        }
        #endregion

    }
}
