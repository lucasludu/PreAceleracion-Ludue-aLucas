using AutoMapper;
using Disney.Dtos.Genre;
using Disney.Services.Interfaces;
using Disney.UOWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Disney.Controllers
{
    [Authorize]
    [Route("api/genres")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ApiExplorerSettings(GroupName = "ApiGenreDisney")]
    public class GenreController : ControllerBase
    {
        private readonly IUnitOfWork _uowork;
        private readonly IGenreService _genreService;
        private readonly IMapper _mapper;

        public GenreController(IUnitOfWork uowork, IGenreService genreService, IMapper mapper)
        {
            _uowork = uowork;
            _genreService = genreService;
            _mapper = mapper;
        }

        /// <summary>
        /// Add Genre
        /// </summary>
        /// <param name="genreCreate"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GenreDto>> Post([FromForm] GenreCreateDto genreCreate)
        {
            GenreDto dto = await _genreService.AddCharacter(genreCreate);
            return Ok(dto);
        }

        /// <summary>
        /// Get All Genre
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<GenreDto>>> GetAllGenres()
        {
            return Ok(await _genreService.GetAllGenres());
        }

        /// <summary>
        /// Add Movie To Genre
        /// </summary>
        /// <param name="genreId"></param>
        /// <param name="movieId"></param>
        /// <returns></returns>
        [HttpPut("{genreId}/movies/{movieId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddMovieToGenre(int genreId, int movieId)
        {
            await _genreService.AddMovieToGenre(genreId, movieId);
            return Ok();
        }

    }
}
