using MovieLibrary.Shared;

namespace MovieLibrary.Server.Interfaces
{
    public interface IMovieService
    {
        Task<List<Movie>> GetMoviesAsync();
        Task<List<Movie>> GetSortMoviesAsync(string order);
        Task<List<Movie>> GetGenreSortMoviesAsync(string genre, int order);
        Task<List<Movie>> SearchMovieAsync(string condition);
        Task<Movie> GetMovieDetailsAsync(int id);
        Task<Movie> InsertMovieAsync(Movie movie);
        Task UpdateMovieAsync(int movieId, Movie movie);
        Task UpdateMovieGenreAsync(int movieId, List<int> selectedGenres);
        Task UpdateMovieActorAsync(int movieId, List<int> selectedActors);
        Task DeleteMovieAsync(int movieId);
    }
}
