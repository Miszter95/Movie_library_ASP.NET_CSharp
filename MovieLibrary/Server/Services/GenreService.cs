using Microsoft.EntityFrameworkCore;
using MovieLibrary.Server.Data;
using MovieLibrary.Server.Interfaces;
using MovieLibrary.Shared;

namespace MovieLibrary.Server.Services
{
    public class GenreService : IGenreService
    {
        private readonly AppDataContext _context;

        public GenreService(IDbContextFactory<AppDataContext> context)
        {
            _context = context.CreateDbContext();
        }

        public async Task<List<Genre>> GetGenresAsync()
        {
            return await _context.Genres.ToListAsync();
        }

        public async Task<Genre> GetGenreDetailsAsync(int id)
        {
            return await _context.Genres
                .FirstOrDefaultAsync(p => p.Id == id)
                ?? throw new Exception("Genre not found!");
        }

        public async Task<Genre> InsertGenreAsync(Genre genre)
        {
            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();
            return genre;
        }

        public async Task UpdateGenreAsync(int genreId, Genre genre)
        {
            var item = _context.Attach(genre);
            item.State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Genres.AnyAsync(m => m.Id == genreId))
                {
                    throw new Exception("Genre not found!");
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task DeleteGenreAsync(int genreId)
        {
            _context.Genres.Remove(
                new Genre(null!, null!) { Id = genreId });

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Genres.AnyAsync(g => g.Id == genreId))
                    throw new Exception("Genre not found");
                else
                    throw;
            }
        }
    }
}
