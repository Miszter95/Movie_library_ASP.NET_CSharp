using MovieLibrary.Server.Interfaces;
using MovieLibrary.Shared;

namespace MovieLibrary.Server.GraphQL
{
    public class MovieQueryResolver
    {
        readonly IMovieService _movieService;

        public MovieQueryResolver(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [GraphQLDescription("Gets the list of movies.")]
        public async Task<List<Movie>> GetMovieList()
        {
            return await _movieService.GetMoviesAsync();
        }

        [GraphQLDescription("Gets a movie.")]
        public async Task<Movie> GetMovie(int id)
        {
            return await _movieService.GetMovieDetailsAsync(id);
        }
    }
}
