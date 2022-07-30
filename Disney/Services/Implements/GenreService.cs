using AutoMapper;
using Disney.Data;
using Disney.Dtos.Genre;
using Disney.Entities;
using Disney.Services.Interfaces;
using Disney.UOWork;
using Disney.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Disney.Services.Implements
{
    public class GenreService : IGenreService
    {
        private readonly IUnitOfWork _uowork;
        private readonly IMapper _mapper;
        private readonly IFileStorage _fileStorage;
        private readonly ApplicationDbContext _context;

        public GenreService(IUnitOfWork uowork, IMapper mapper, IFileStorage fileStorage, ApplicationDbContext context)
        {
            _uowork = uowork;
            _mapper = mapper;
            _fileStorage = fileStorage;
            _context = context;
        }

        #region AddCharacter
        public async Task<GenreDto> AddCharacter(GenreCreateDto genreCreateDto)
        {
            if (genreCreateDto == null)
            {
                throw new ArgumentOutOfRangeException("No character-create-dto found.");
            }

            var entity = _mapper.Map<Genre>(genreCreateDto);

            if (genreCreateDto.Image != null)
            {
                string imageUrl = await SaveImage(genreCreateDto.Image);
                entity.Image = imageUrl;
            }
            _uowork.GenreRepo.Insert(entity);
            _uowork.Save();

            var dto = _mapper.Map<GenreDto>(entity);
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
                ConstantApplications.FileContainer.FileGenre,
                Guid.NewGuid().ToString()
            );
        }
        #endregion

        #region GetAll
        public async Task<IList<GenreDto>> GetAllGenres()
        {
            var existingGenres = await _context.Genres.ToListAsync();
            var result = existingGenres.Select(g => _mapper.Map<GenreDto>(g)).ToList();
            return result;
        }
        #endregion

        #region AddMovieToGenre
        public async Task AddMovieToGenre(int genreId, int movieId)
        {
            var existingGenre = await _context.Genres.FirstOrDefaultAsync(g => g.Id == genreId);

            if (existingGenre == null)
                throw new ArgumentOutOfRangeException("Genre not found");

            var existingMovie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == movieId);

            if (existingMovie == null)
                throw new ArgumentOutOfRangeException("Movie not found");

            if (existingGenre.Movies == null)
                existingGenre.Movies = new List<Movie>();

            existingGenre.Movies.Add(existingMovie);

            await _context.SaveChangesAsync();
        }
        #endregion

    }
}
