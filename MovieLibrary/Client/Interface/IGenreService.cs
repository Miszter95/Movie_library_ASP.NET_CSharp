using MovieLibrary.Shared;

namespace MovieLibrary.Client.Interface
{
    public interface IGenreService
    {
        List<Genre> Genres { get; set; }

        Task LoadGenres();
        Task<Genre> LoadGenreDetails(int id);
        Task CreateGenre(Genre genre);
        Task UpdateGenre(Genre genre, int id);
        Task DeleteGenre(int id);
    }
}
