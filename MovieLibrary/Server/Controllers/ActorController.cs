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
    public class ActorController : ControllerBase
    {
        private readonly IActorService _actorService;

        public ActorController(IActorService actorService)
        {
            _actorService = actorService;
        }

        /// <summary>
        /// Get all actor
        /// </summary>
        /// <returns>Returns all actor</returns>
        /// <response code="200">Listing successful</response>
        [HttpGet]
        public async Task<ActionResult<List<Actor>>> GetAllActor()
        {
            return (await _actorService.GetActorsAsync()).ToList();
        }

        /// <summary>
        /// Get all actor sorted by the given sort parameter
        /// </summary>
        /// <param name="order">Actor's sort parameter</param>
        /// <returns>Returns all actor sorted by the given sort parameter</returns>
        /// <response code="200">Listing successful</response>
        [HttpGet("order/{order}")]
        public async Task<ActionResult<List<Actor>>> GetSortAllActor(string order)
        {
            return await _actorService.GetSortActorsAsync(order);
        }

        /// <summary>
        /// Get a specific actor with the given name
        /// </summary>
        /// <param name="condition">Actor's name</param>
        /// <returns>Returns a specific actor with the given name</returns>
        /// <response code="200">Listing successful</response>
        [HttpGet("search/{condition}")]
        public async Task<ActionResult<List<Actor>>> SearchActors(string condition)
        {
            return (await _actorService.SearchActorAsync(condition));
        }

        /// <summary>
        /// Get a specific actor with the given identifier
        /// </summary>
        /// <param name="id">Actor's identifier</param>
        /// <returns>Returns a specific actor with the given identifier</returns>
        /// <response code="200">Listing successful</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Actor>> GetActor(int id)
        {
            try
            {
                return await _actorService.GetActorDetailsAsync(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return NotFound(e.Message);
            }
        }

        /// <summary>
        /// Create a new actor
        /// </summary>
        /// <param name="actor">Actor to be created</param>
        /// <returns>Returns the created actor</returns>
        /// <response code="201">Create successful</response>
        [HttpPost]
        public async Task<ActionResult<Actor>> InsertActor([FromBody] Actor actor)
        {
            var created = await _actorService.InsertActorAsync(actor);
            return CreatedAtAction(nameof(GetActor), new { id = created.Id }, created);
        }

        /// <summary>
        /// Update a specific actor with the given identifier
        /// </summary>
        /// <param name="id">Actor's identifier</param>
        /// <param name="actor">Actor to be updated</param>
        /// <returns>Returns a NoContent object</returns>
        /// <response code="204">Update successful</response>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateActor(int id, [FromBody] Actor actor)
        {
            await _actorService.UpdateActorAsync(id, actor);
            return NoContent();
        }

        /// <summary>
        /// Delete a specific actor with the given identifier
        /// </summary>
        /// <param name="id">Actor's identifier</param>
        /// <returns>Returns a NoContent object</returns>
        /// <response code="204">Delete successful</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteActor(int id)
        {
            await _actorService.DeleteActorAsync(id);
            return NoContent();
        }
    }
}
