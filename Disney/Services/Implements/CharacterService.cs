using AutoMapper;
using Disney.Data;
using Disney.Dtos.Character;
using Disney.Entities;
using Disney.Services.Interfaces;
using Disney.UOWork;
using Disney.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Disney.Services.Implements
{
    public class CharacterService : ICharacterService
    {
        private readonly IUnitOfWork _uowork;
        private readonly IMapper _mapper;
        private readonly IFileStorage _fileStorage;
        private readonly ApplicationDbContext _context;

        public CharacterService(IUnitOfWork uowork, IMapper mapper, IFileStorage fileStorage, ApplicationDbContext context)
        {
            _uowork = uowork;
            _mapper = mapper;
            _fileStorage = fileStorage;
            _context = context;
        }

        #region Add Character
        public async Task<CharacterDto> AddCharacter(CharacterCreateDto characterCreateDto)
        {
            if (characterCreateDto == null)
            {
                throw new ArgumentOutOfRangeException("No character-create-dto found.");
            }

            var entity = _mapper.Map<Character>(characterCreateDto);

            if (characterCreateDto.Image != null)
            {
                string imageUrl = await SaveImage(characterCreateDto.Image);
                entity.Image = imageUrl;
            }
            _uowork.CharacterRepo.Insert(entity);
            _uowork.Save();

            var dto = _mapper.Map<CharacterDto>(entity);
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
                ConstantApplications.FileContainer.FileCharacters,
                Guid.NewGuid().ToString()
            );
        }
        #endregion

        #region GeyById
        public async Task<CharacterGetDto> GetById(int id)
        {
            var existingCharacter = _uowork.CharacterRepo.GetById(id);

            if (existingCharacter == null)
                return null;

            return _mapper.Map<CharacterGetDto>(existingCharacter);
        }
        #endregion

        #region GetAll
        public async Task<IList<CharacterListItemDto>> GetAll()
        {
            var characters = _uowork.CharacterRepo.GetAll();
            var result = characters.Select(c => _mapper.Map<CharacterListItemDto>(c)).ToList();

            return result;
        }
        #endregion

        #region GetByName
        public async Task<CharacterListItemDto> GetByName(string name)
        {
            var existingCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Name.ToLower().Equals(name.ToLower()));

            if (existingCharacter == null)
                return null;

            var result = _mapper.Map<CharacterListItemDto>(existingCharacter);
            return result;
        }
        #endregion

        #region GetByAge
        public async Task<IList<CharacterListItemDto>> GetCharactersByAge(int age)
        {
            var existingCharacters = await _context.Characters.Where(c => c.Age == age).ToListAsync();

            if (existingCharacters == null)
                return null;

            var result = existingCharacters.Select(c => _mapper.Map<CharacterListItemDto>(c)).ToList();
            return result;
        }
        #endregion

        #region GetFromMovie
        public async Task<IList<CharacterListItemDto>> GetCharactersFromMovie(int movieId)
        {
            var existingMovie = await _context.Movies
                                .Include(m => m.Characters)
                                .FirstOrDefaultAsync(m => m.Id == movieId);

            if (existingMovie == null)
                return new CharacterListItemDto[0];

            var result = existingMovie.Characters
                            .Select(c => _mapper.Map<CharacterListItemDto>(c))
                            .ToList();

            return result;
        }
        #endregion

        #region Update
        public async Task<CharacterCreateDto> UpdateCharacter(int id, CharacterCreateDto characterUpdate)
        {
            var entity = await _context.Characters.FirstOrDefaultAsync(a => a.Id == id);
            if (entity == null)
            {
                throw new ArgumentOutOfRangeException("No character-create-dto found.");
            }

            _mapper.Map(characterUpdate, entity);

            if(characterUpdate.Image != null)
            {
                if(!string.IsNullOrEmpty(entity.Image))
                {
                    await _fileStorage.Delete(entity.Image, ConstantApplications.FileContainer.FileCharacters);
                }
                string pictureUrl = await SaveImage(characterUpdate.Image);
                entity.Image = pictureUrl;
            }
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChangesAsync();

            return characterUpdate;
        }
        #endregion

    }
}
