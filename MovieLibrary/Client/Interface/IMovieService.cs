using MovieLibrary.Shared;
using static MovieLibrary.Client.Pages.EditMovie;

namespace MovieLibrary.Client.Interface
{
    public interface IMovieService
    {
        List<Movie> Movies { get; set; }

        Task LoadSortMovies(string order);
        Task LoadGenreSortMovies(string genre, int order);
        Task<Movie> LoadMovieDetails(int id);
        Task CreateMovie(Movie movie);
        Task UpdateMovie(Movie movie, int id);
        Task UpdateMovieGenre(int id, List<int> selectedGenres);
        Task UpdateMovieActor(int id, List<int> selectedActors);
        Task DeleteMovie(int id);
        Task<List<Movie>> SearhMovies(string condition);
    }
}
