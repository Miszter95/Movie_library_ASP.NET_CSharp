using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using MovieLibrary.Server.Interfaces;
using MovieLibrary.Server.Services;
using MovieLibrary.Shared;

namespace MovieLibrary.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes")]
    public class GenreController : ControllerBase
    {
        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        /// <summary>
        /// Get all genre
        /// </summary>
        /// <returns>Returns all genre</returns>
        /// <response code="200">Listing successful</response>
        [HttpGet]
        public async Task<ActionResult<List<Genre>>> GetAllGenre()
        {
            return (await _genreService.GetGenresAsync()).ToList();
        }

        /// <summary>
        /// Get a specific genre with the given identifier
        /// </summary>
        /// <param name="id">Genre's identifier</param>
        /// <returns>Returns a specific genre with the given identifier</returns>
        /// <response code="200">Listing successful</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Genre>> GetGenre(int id)
        {
            try
            {
                return await _genreService.GetGenreDetailsAsync(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return NotFound(e.Message);
            }
        }

        /// <summary>
        /// Create a new genre
        /// </summary>
        /// <param name="genre">Genre to be created</param>
        /// <returns>Returns the created genre</returns>
        /// <response code="201">Create successful</response>
        [HttpPost]
        public async Task<ActionResult<Genre>> InsertGenre([FromBody] Genre genre)
        {
            var created = await _genreService.InsertGenreAsync(genre);
            return CreatedAtAction(nameof(GetGenre), new { id = created.Id }, created);
        }

        /// <summary>
        /// Update a specific genre with the given identifier
        /// </summary>
        /// <param name="id">Genre's identifier</param>
        /// <param name="genre">Genre to be updated</param>
        /// <returns>Returns a NoContent object</returns>
        /// <response code="204">Update successful</response>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateGenre(int id, [FromBody] Genre genre)
        {
            await _genreService.UpdateGenreAsync(id, genre);
            return NoContent();
        }

        /// <summary>
        /// Delete a specific genre with the given identifier
        /// </summary>
        /// <param name="id">Genre's identifier</param>
        /// <returns>Returns a NoContent object</returns>
        /// <response code="204">Delete successful</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteGenre(int id)
        {
            await _genreService.DeleteGenreAsync(id);
            return NoContent();
        }
    }
}
