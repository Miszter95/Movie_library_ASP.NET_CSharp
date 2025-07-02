using MovieLibrary.Client.Interface;
using MovieLibrary.Shared;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace MovieLibrary.Client.Service
{
    public class GenreService : IGenreService
    {
        private readonly HttpClient _httpClient;
        public List<Genre> Genres { get; set; } = new List<Genre>();

        public GenreService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task LoadGenres()
        {
            Genres = await _httpClient.GetFromJsonAsync<List<Genre>>("api/Genre") ?? new List<Genre>();
        }

        public async Task<Genre> LoadGenreDetails(int id)
        {
            return await _httpClient.GetFromJsonAsync<Genre>($"api/Genre/{id}") ?? new Genre("","");
        }

        public async Task CreateGenre(Genre genre)
        {
            await _httpClient.PostAsJsonAsync<Genre>($"api/Genre", genre);
        }

        public async Task UpdateGenre(Genre genre, int id)
        {
            await _httpClient.PutAsJsonAsync($"api/Genre/{id}", genre);
        }

        public async Task DeleteGenre(int id)
        {
            await _httpClient.DeleteAsync($"api/Genre/{id}");
        }
    }
}
