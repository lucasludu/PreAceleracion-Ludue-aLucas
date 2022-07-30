using AutoMapper;
using Disney.Dtos.Movies;
using Disney.Services.Interfaces;
using Disney.UOWork;
using Disney.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Disney.Controllers
{
    [Authorize]
    [Route("api/movies")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ApiExplorerSettings(GroupName = "ApiMovieDisney")]
    public class MovieController : ControllerBase
    {
        private readonly IUnitOfWork _uowork;
        private readonly IMovieService _movieService;
        private readonly IMapper _mapper;

        public MovieController(IUnitOfWork uowork, IMovieService movieService, IMapper mapper)
        {
            _uowork = uowork;
            _movieService = movieService;
            _mapper = mapper;
        }

        /// <summary>
        /// Add Movie
        /// </summary>
        /// <param name="movieCreate"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MovieDto>> Post([FromForm] MovieCreateDto movieCreate)
        {
            MovieDto dto = await _movieService.AddMovie(movieCreate);
            return Ok(dto);
        }

        /// <summary>
        /// Get All Movies
        /// </summary>
        /// <param name="name"></param>
        /// <param name="genre"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IList<MovieListDto>>> AllMovies(string name = "", int genre = 0, Order order = Order.None)
        {
            if (!name.Equals(string.Empty))
                return Ok(await _movieService.GetMoviesByTitle(name));

            if (genre != 0)
                return Ok(await _movieService.GetMoviesByGenre(genre));

            if (order != Order.None)
                return Ok(await _movieService.GetMoviesByReleased(order));

            return Ok(await _movieService.GetAllMovies());
        }

        /// <summary>
        /// Get Movie Details by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/details")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MovieDetailsDto>> GetMovieDetails(int id)
        {
            return Ok(await _movieService.GetMovieDetails(id));
        }

        /// <summary>
        /// Add Character To Movie
        /// </summary>
        /// <param name="movieId"></param>
        /// <param name="characterId"></param>
        /// <returns></returns>
        [HttpPut("{movieId}/characters/{characterId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddCharacterToMovie(int movieId, int characterId)
        {
            await _movieService.AddCharacterToMovie(movieId, characterId);
            return Ok();
        }

        /// <summary>
        /// Update Movie
        /// </summary>
        /// <param name="id"></param>
        /// <param name="movieUpdate"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MovieCreateDto>> Put(int id, [FromForm] MovieCreateDto movieUpdate)
        {
            var result = await _movieService.UpdateMovie(id, movieUpdate);
            return Ok(result);
        }

        /// <summary>
        /// Delete Movie by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(int id)
        {
            _uowork.MovieRepo.Delete(id);
            _uowork.Save();
            return NoContent();
        }
    }
}
