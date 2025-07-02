using MovieLibrary.Client.Interface;
using MovieLibrary.Shared;
using System.Net.Http.Json;

namespace MovieLibrary.Client.Service
{
    public class MovieService : IMovieService
    {
        private readonly HttpClient _httpClient;
        public List<Movie> Movies { get; set; } = new List<Movie>();

        public MovieService(HttpClient httpclient)
        {
            _httpClient = httpclient;
        }

        public async Task LoadSortMovies(string order = "")
        {
            if(order != "")
            {
                Movies = await _httpClient.GetFromJsonAsync<List<Movie>>($"api/Movie/order/{order}") ?? new List<Movie>();
            }
            else
            {
                Movies = await _httpClient.GetFromJsonAsync<List<Movie>>("api/Movie") ?? new List<Movie>();
            }
        }

        public async Task LoadGenreSortMovies(string genre, int order)
        {
            Movies = await _httpClient.GetFromJsonAsync<List<Movie>>($"api/Movie/order/bygenre/{genre}/{order}") ?? new List<Movie>();
        }

        public async Task<Movie> LoadMovieDetails(int id)
        {
            return await _httpClient.GetFromJsonAsync<Movie>($"api/Movie/{id}") ?? new Movie("", "","","");
        }

        public async Task CreateMovie(Movie movie)
        {
            await _httpClient.PostAsJsonAsync<Movie>($"api/Movie", movie);
        }

        public async Task UpdateMovie(Movie movie, int id)
        {
            await _httpClient.PutAsJsonAsync($"api/Movie/{id}", movie);
        }

        public async Task UpdateMovieGenre(int id, List<int> selectedGenres)
        {
            await _httpClient.PutAsJsonAsync($"api/Movie/{id}/addgenre", selectedGenres);
        }

        public async Task UpdateMovieActor(int id, List<int> selectedActors)
        {
            await _httpClient.PutAsJsonAsync($"api/Movie/{id}/addactor", selectedActors);
        }

        public async Task DeleteMovie(int id)
        {
            await _httpClient.DeleteAsync($"api/Movie/{id}");
        }

        public async Task<List<Movie>> SearhMovies(string condition)
        {
            return await _httpClient.GetFromJsonAsync<List<Movie>>($"api/Movie/search/{condition}") ?? new List<Movie>();
        }
    }
}
