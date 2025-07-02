using MovieLibrary.Shared;

namespace MovieLibrary.Client.Interface
{
    public interface IActorService
    {
        List<Actor> Actors { get; set; }

        Task LoadSortActors(string order = "");
        Task<Actor> LoadActorDetails(int id);
        Task CreateActor(Actor actor);
        Task UpdateActor(Actor actor, int id);
        Task DeleteActor(int id);
        Task<List<Actor>> SearhActors(string condition);
    }
}
