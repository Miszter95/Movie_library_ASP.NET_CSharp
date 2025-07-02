using MovieLibrary.Shared;

namespace MovieLibrary.Server.Interfaces
{
    public interface IGenreService
    {
        Task<List<Genre>> GetGenresAsync();
        Task<Genre> GetGenreDetailsAsync(int id);
        Task<Genre> InsertGenreAsync(Genre genre);
        Task UpdateGenreAsync(int genreId, Genre genre);
        Task DeleteGenreAsync(int genreId);
    }
}
