using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using MovieLibrary.Server.Data;
using MovieLibrary.Server.Interfaces;
using MovieLibrary.Shared;

namespace MovieLibrary.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        /// <summary>
        /// Get all movie
        /// </summary>
        /// <returns>Returns all movie</returns>
        /// <response code="200">Listing successful</response>
        [HttpGet]
        public async Task<ActionResult<List<Movie>>> GetAllMovie()
        {
            return await _movieService.GetMoviesAsync();
        }

        /// <summary>
        /// Get all movie sorted by the given sort parameter
        /// </summary>
        /// <param name="order">Movie's sort parameter</param>
        /// <returns>Returns all movie sorted by the given sort parameter</returns>
        /// <response code="200">Listing successful</response>
        [HttpGet("order/{order}")]
        public async Task<ActionResult<List<Movie>>> GetSortAllMovie(string order)
        {
            return await _movieService.GetSortMoviesAsync(order);
        }

        /// <summary>
        /// Get all movie sorted by the given genre then sorted by the given sort parameter
        /// </summary>
        /// <param name="genre">Movie's genre sort parameter</param>
        /// <param name="order">Movie's sort parameter</param>
        /// <returns>Returns all movie sorted by the given genre then sorted by the given sort parameter</returns>
        /// <response code="200">Listing successful</response>
        [HttpGet("order/bygenre/{genre}/{order}")]
        public async Task<ActionResult<List<Movie>>> GetGenreSortAllMovie(string genre, int order)
        {
            return await _movieService.GetGenreSortMoviesAsync(genre, order);
        }

        /// <summary>
        /// Get a specific movie with the given title
        /// </summary>
        /// <param name="condition">Movie's title</param>
        /// <returns>Returns a specific movie with the given title</returns>
        /// <response code="200">Listing successful</response>
        [HttpGet("search/{condition}")]
        public async Task<ActionResult<List<Movie>>> SearchMovies(string condition)
        {
            return (await _movieService.SearchMovieAsync(condition));
        }

        /// <summary>
        /// Get a specific movie with the given identifier
        /// </summary>
        /// <param name="id">Movie's identifier</param>
        /// <returns>Returns a specific movie with the given identifier</returns>
        /// <response code="200">Listing successful</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            try
            {
                return await _movieService.GetMovieDetailsAsync(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return NotFound(e.Message);
            }
        }

        /// <summary>
        /// Create a new movie
        /// </summary>
        /// <param name="movie">Movie to be created</param>
        /// <returns>Returns the created movie</returns>
        /// <response code="201">Create successful</response>
        [HttpPost]
        public async Task<ActionResult<Movie>> InsertMovie([FromBody] Movie movie)
        {
            var created = await _movieService.InsertMovieAsync(movie);
            return CreatedAtAction(nameof(GetMovie), new { id = created.Id }, created);
        }

        /// <summary>
        /// Update a specific movie with the given identifier
        /// </summary>
        /// <param name="id">Movie's identifier</param>
        /// <param name="movie">Movie to be updated</param>
        /// <returns>Returns a NoContent object</returns>
        /// <response code="204">Update successful</response>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateMovie(int id, [FromBody] Movie movie)
        {
            await _movieService.UpdateMovieAsync(id, movie);
            return NoContent();
        }

        /// <summary>
        /// Update a specific movie's genres with the given identifier
        /// </summary>
        /// <param name="id">Movie's identifier</param>
        /// <param name="selectedgenres">Selected genres to update for the film</param>
        /// <returns>Returns a NoContent object</returns>
        /// <response code="204">Update successful</response>
        [HttpPut("{id}/addgenre")]
        public async Task<ActionResult> UpdateMovieGenre(int id, List<int> selectedgenres)
        {
            await _movieService.UpdateMovieGenreAsync(id, selectedgenres);
            return NoContent();
        }

        /// <summary>
        /// Update a specific movie's actors with the given identifier
        /// </summary>
        /// <param name="id">Movie's identifier</param>
        /// <param name="selectedactors">Selected actors to update for the film</param>
        /// <returns>Returns a NoContent object</returns>
        /// <response code="204">Update successful</response>
        [HttpPut("{id}/addactor")]
        public async Task<ActionResult> UpdateMovieActor(int id, List<int> selectedactors)
        {
            await _movieService.UpdateMovieActorAsync(id, selectedactors);
            return NoContent();
        }

        /// <summary>
        /// Delete a specific movie with the given identifier
        /// </summary>
        /// <param name="id">Movie's identifier</param>
        /// <returns>Returns a NoContent object</returns>
        /// <response code="204">Delete successful</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMovie(int id)
        {
            await _movieService.DeleteMovieAsync(id);
            return NoContent();
        }
    }
}
