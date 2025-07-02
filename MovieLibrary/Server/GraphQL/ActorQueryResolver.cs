using MovieLibrary.Server.Interfaces;
using MovieLibrary.Shared;

namespace MovieLibrary.Server.GraphQL
{
    public class ActorQueryResolver
    {

        readonly IActorService _actorService;

        public ActorQueryResolver(IActorService actorService)
        {
            _actorService = actorService;
        }

        [GraphQLDescription("Gets the list of actors.")]
        public async Task<List<Actor>> GetActorList()
        {
            return await _actorService.GetActorsAsync();
        }

        [GraphQLDescription("Gets an actor.")]
        public async Task<Actor> GetActor(int id)
        {
            return await _actorService.GetActorDetailsAsync(id);
        }
    }
}
