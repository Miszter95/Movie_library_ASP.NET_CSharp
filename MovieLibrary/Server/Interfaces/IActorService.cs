using MovieLibrary.Shared;

namespace MovieLibrary.Server.Interfaces
{
    public interface IActorService
    {
        Task<List<Actor>> GetActorsAsync();
        Task<List<Actor>> GetSortActorsAsync(string order);
        Task<List<Actor>> SearchActorAsync(string condition);
        Task<Actor> GetActorDetailsAsync(int id);
        Task<Actor> InsertActorAsync(Actor actor);
        Task UpdateActorAsync(int actorId, Actor actor);
        Task DeleteActorAsync(int actorId);
    }
}
