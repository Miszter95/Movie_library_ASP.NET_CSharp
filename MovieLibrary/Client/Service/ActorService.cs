using MovieLibrary.Client.Interface;
using MovieLibrary.Shared;
using System.Net.Http.Json;

namespace MovieLibrary.Client.Service
{
    public class ActorService : IActorService
    {

        private readonly HttpClient _httpClient;
        public List<Actor> Actors { get; set; } = new List<Actor>();

        public ActorService(HttpClient httpclient)
        {
            _httpClient = httpclient;
        }

        public async Task LoadSortActors(string order = "")
        {
            if(order != "")
            {
                Actors = await _httpClient.GetFromJsonAsync<List<Actor>>($"api/Actor/order/{order}") ?? new List<Actor>();
            }
            else
            {
                Actors = await _httpClient.GetFromJsonAsync<List<Actor>>("api/Actor") ?? new List<Actor>();
            }
        }

        public async Task<Actor> LoadActorDetails(int id)
        {
            return await _httpClient.GetFromJsonAsync<Actor>($"api/Actor/{id}") ?? new Actor("", "","");
        }

        public async Task CreateActor(Actor actor)
        {
            await _httpClient.PostAsJsonAsync<Actor>($"api/Actor", actor);
        }

        public async Task UpdateActor(Actor actor, int id)
        {
            await _httpClient.PutAsJsonAsync($"api/Actor/{id}", actor);
        }

        public async Task DeleteActor(int id)
        {
            await _httpClient.DeleteAsync($"api/Actor/{id}");
        }

        public async Task<List<Actor>> SearhActors(string condition)
        {
            return await _httpClient.GetFromJsonAsync<List<Actor>>($"api/Actor/search/{condition}") ?? new List<Actor>();
        }
    }
}
