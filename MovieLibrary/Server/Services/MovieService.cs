using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using MovieLibrary.Client.Pages;
using MovieLibrary.Server.Data;
using MovieLibrary.Server.Interfaces;
using MovieLibrary.Shared;

namespace MovieLibrary.Server.Services
{
    public class MovieService : IMovieService
    {
        private readonly AppDataContext _context;

        public MovieService(IDbContextFactory<AppDataContext> context)
        {
            _context = context.CreateDbContext();
        }

        public async Task<List<Movie>> GetMoviesAsync()
        {
            return await _context.Movies.Include(g => g.Genres).Include(a => a.Actors).AsSplitQuery().ToListAsync();
        }

        public async Task<List<Movie>> GetSortMoviesAsync(string order)
        {

            if (order == "name-aorder")
            {
                return await _context.Movies.Include(g => g.Genres).Include(a => a.Actors).AsSplitQuery().OrderBy(m => m.Title).ToListAsync();
            }
            else if (order == "name-deorder")
            {
                return await _context.Movies.Include(g => g.Genres).Include(a => a.Actors).AsSplitQuery().OrderByDescending(m => m.Title).ToListAsync();
            }
            else if (order == "release-aorder")
            {
                return await _context.Movies.Include(g => g.Genres).Include(a => a.Actors).AsSplitQuery().OrderBy(m => m.ReleaseDate).ToListAsync();
            }
            else if (order == "release-deorder")
            {
                return await _context.Movies.Include(g => g.Genres).Include(a => a.Actors).AsSplitQuery().OrderByDescending(m => m.ReleaseDate).ToListAsync();
            }
            else if (order == "length-aorder")
            {
                return await _context.Movies.Include(g => g.Genres).Include(a => a.Actors).AsSplitQuery().OrderBy(m => m.Length).ToListAsync();
            }
            else if (order == "length-deorder")
            {
                return await _context.Movies.Include(g => g.Genres).Include(a => a.Actors).AsSplitQuery().OrderByDescending(m => m.Length).ToListAsync();
            }
            else
            {
                return await _context.Movies.Include(g => g.Genres).Include(a => a.Actors).AsSplitQuery().Where(m => m.Genres.Any(g => g.Name == order)).ToListAsync();
            }
        }

        public async Task<List<Movie>> GetGenreSortMoviesAsync(string genre, int order)
        {
            if (order == 1)
            {
                return await _context.Movies.Include(g => g.Genres).Include(a => a.Actors).AsSplitQuery().Where(m => m.Genres.Any(g => g.Name == genre)).OrderBy(m => m.Title).ToListAsync();
            }
            else if (order == 2)
            {
                return await _context.Movies.Include(g => g.Genres).Include(a => a.Actors).AsSplitQuery().Where(m => m.Genres.Any(g => g.Name == genre)).OrderByDescending(m => m.Title).ToListAsync();
            }
            else if (order == 3)
            {
                return await _context.Movies.Include(g => g.Genres).Include(a => a.Actors).AsSplitQuery().Where(m => m.Genres.Any(g => g.Name == genre)).OrderBy(m => m.ReleaseDate).ToListAsync();
            }
            else if (order == 4)
            {
                return await _context.Movies.Include(g => g.Genres).Include(a => a.Actors).AsSplitQuery().Where(m => m.Genres.Any(g => g.Name == genre)).OrderByDescending(m => m.ReleaseDate).ToListAsync();
            }
            else if (order == 5)
            {
                return await _context.Movies.Include(g => g.Genres).Include(a => a.Actors).AsSplitQuery().Where(m => m.Genres.Any(g => g.Name == genre)).OrderBy(m => m.Length).ToListAsync();
            }
            else
            {
                return await _context.Movies.Include(g => g.Genres).Include(a => a.Actors).AsSplitQuery().Where(m => m.Genres.Any(g => g.Name == genre)).OrderByDescending(m => m.Length).ToListAsync();
            }
        }

        public async Task<List<Movie>> SearchMovieAsync(string condition)
        {
            return await _context.Movies.Where(m => m.Title.ToLower().Contains(condition.ToLower())).ToListAsync();
        }

        public async Task<Movie> GetMovieDetailsAsync(int id)
        {
            return await _context.Movies.Include(g => g.Genres).Include(a => a.Actors)
                .AsSplitQuery().FirstOrDefaultAsync(m => m.Id == id)
                ?? throw new Exception("Movie not found!");
        }

        public async Task<Movie> InsertMovieAsync(Movie movie)
        {
            movie = await MoiveGenre(movie);
            movie = await MovieActor(movie);

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
            return movie;
        }

        public async Task UpdateMovieAsync(int movieId, Movie movie)
        {

            var item = _context.Attach(movie);
            item.State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Movies.AnyAsync(m => m.Id == movieId))
                {
                    throw new Exception("Movie not found!");
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task UpdateMovieGenreAsync(int movieId, List<int> selectedGenre)
        {
            var movieToUpdate = _context.Movies.Include(g => g.Genres).Include(a => a.Actors).FirstOrDefault(m => m.Id == movieId) ??
                throw new Exception("Movie not found!");

            UpdateMovieGenres(selectedGenre, movieToUpdate);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateMovieActorAsync(int movieId, List<int> selectedActors)
        {
            var movieToUpdate = _context.Movies.Include(g => g.Genres).Include(a => a.Actors).FirstOrDefault(m => m.Id == movieId) ??
                throw new Exception("Movie not found!");

            UpdateMovieActors(selectedActors, movieToUpdate);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteMovieAsync(int movieId)
        {
            _context.Movies.Remove(
                new Movie(null!, null!, null!, null!) { Id = movieId });

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Movies.AnyAsync(m => m.Id == movieId))
                    throw new Exception("Movie not found");
                else
                    throw;
            }
        }

        public async Task<Movie> MovieActor(Movie movie)
        {
            List<int> actorsid = new List<int>();

            foreach (var actor in movie.Actors)
            {
                actorsid.Add(actor.Id);
            }

            movie.Actors.Clear();

            foreach (var actorid in actorsid)
            {
                movie.Actors.Add(await _context.Actors
                .FirstOrDefaultAsync(a => a.Id == actorid) ??
                throw new Exception("Actor not found!"));
            }

            return movie;
        }

        public async Task<Movie> MoiveGenre(Movie movie)
        {
            List<int> genresid = new List<int>();

            foreach (var genre in movie.Genres)
            {
                genresid.Add(genre.Id);
            }

            movie.Genres.Clear();

            foreach (var genreid in genresid)
            {
                movie.Genres.Add(await _context.Genres
                .FirstOrDefaultAsync(g => g.Id == genreid) ??
                throw new Exception("Genre not found!"));
            }

            return movie;
        }

        public void UpdateMovieGenres(List<int> selectedGenre, Movie movieToUpdate)
        {
            List<int> selectedGenres = new List<int>(selectedGenre);
            List<int> movieGenres = new List<int>(movieToUpdate.Genres.Select(g => g.Id));

            foreach (var genre in _context.Genres)
            {
                if (selectedGenres.Contains(genre.Id))
                {
                    if (!movieGenres.Contains(genre.Id))
                    {
                        movieToUpdate.Genres.Add(genre);
                    }
                }
                else
                {
                    if (movieGenres.Contains(genre.Id))
                    {
                        movieToUpdate.Genres.Remove(genre);
                    }
                }
            }
        }

        public void UpdateMovieActors(List<int> selectedActor, Movie movieToUpdate)
        {
            List<int> selectedActors = new List<int>(selectedActor);
            List<int> movieActors = new List<int>(movieToUpdate.Actors.Select(g => g.Id));

            foreach (var actor in _context.Actors)
            {
                if (selectedActors.Contains(actor.Id))
                {
                    if (!movieActors.Contains(actor.Id))
                    {
                        movieToUpdate.Actors.Add(actor);
                    }
                }
                else
                {
                    if (movieActors.Contains(actor.Id))
                    {
                        movieToUpdate.Actors.Remove(actor);
                    }
                }
            }
        }
    }
}
