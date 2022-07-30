using AutoMapper;
using Disney.Dtos.Character;
using Disney.Services.Interfaces;
using Disney.UOWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Disney.Controllers
{
    [Authorize]
    [Route("api/characters")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ApiExplorerSettings(GroupName = "ApiCharacterDisney")]
    public class CharacterController : ControllerBase
    {
        private readonly IUnitOfWork _uowork;
        private readonly ICharacterService _characterService;
        private readonly IMapper _mapper;

        public CharacterController(IUnitOfWork uowork, ICharacterService characterService, IMapper mapper)
        {
            _uowork = uowork;
            _characterService = characterService;
            _mapper = mapper;
        }

        /// <summary>
        /// Add Character
        /// </summary>
        /// <param name="characterCreate"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CharacterDto>> Post([FromForm] CharacterCreateDto characterCreate)
        {
            CharacterDto dto = await _characterService.AddCharacter(characterCreate);
            return Ok(dto);
        }

        /// <summary>
        /// Get Character by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CharacterGetDto>> GetById(int id)
        {
            return Ok(await _characterService.GetById(id));
        }

        /// <summary>
        /// Get Character eby name | age | movies
        /// </summary>
        /// <param name="name"></param>
        /// <param name="age"></param>
        /// <param name="movies"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IList<CharacterListItemDto>>> GetCharacters(string name = "", int age = 0, int movies = 0)
        {
            if (!name.Equals(string.Empty))
            {
                var existingCharacter = await _characterService.GetByName(name);
                return existingCharacter == null ? new CharacterListItemDto[0] : new[] { existingCharacter };
            }

            if (age != 0)
                return Ok(await _characterService.GetCharactersByAge(age));

            if (movies != 0)
                return Ok(await _characterService.GetCharactersFromMovie(movies));

            return Ok(await _characterService.GetAll());
        }

        /// <summary>
        /// Update Character
        /// </summary>
        /// <param name="id"></param>
        /// <param name="characterUpdate"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<CharacterCreateDto>> Put(int id, [FromForm] CharacterCreateDto characterUpdate)
        {
            var result = await _characterService.UpdateCharacter(id, characterUpdate);
            return Ok(result);
        }

        /// <summary>
        /// Delete Character By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(int id)
        {
            _uowork.CharacterRepo.Delete(id);
            _uowork.Save();
            return NoContent();
        }

    }
}
