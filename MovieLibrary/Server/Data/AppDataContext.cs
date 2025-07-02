using Microsoft.EntityFrameworkCore;
using MovieLibrary.Shared;

namespace MovieLibrary.Server.Data
{
    public class AppDataContext : DbContext
    {
        public AppDataContext(DbContextOptions<AppDataContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Movie>().HasData(
                new Movie("Ironman", "Jon Favreau", "Mark Fergus, Hawk Ostby, Art Marcum", "Tony Stark. Genius, billionaire, playboy, philanthropist. Son of legendary inventor and weapons contractor Howard Stark.") { Id = 1, Length = 120, ReleaseDate = 2010, Certificate = 12, MetaScore = 79},
                new Movie("Batman", "Matt Reeves", "Matt Reeves, Peter Craig, Bob Kane", "Two years of nights have turned Bruce Wayne into a nocturnal animal.") { Id = 2, Length = 140, ReleaseDate = 2022, Certificate = 16, MetaScore = 72 },
                new Movie("Captain Marvel", "Anna Boden, Ryan Fleck", "Anna Boden, Ryan Fleck, Geneva Robertson-Dworet", "After crashing an experimental aircraft, Air Force pilot Carol Danvers is discovered by the Kree and trained as a member of the elite Starforce Military under the command of her mentor Yon-Rogg.") { Id = 3, Length = 150, ReleaseDate = 2023, Certificate = 12, MetaScore = 64 });

            modelBuilder.Entity<Actor>().HasData(
                new Actor("Robert Donly Junior", "New York-Manhattan", "Sarah Jessica Parker") { Id = 1, Born = new DateTime(1965,4,4), Height = 160});

            modelBuilder.Entity<Genre>().HasData(
                new Genre("Action", "Movies in the action genre are fast-paced and include a lot of action like fight scenes, chase scenes, and slow-motion shots.") { Id = 1},
                new Genre("Drama", "The drama genre features stories with high stakes and many conflicts.") { Id = 2});

            modelBuilder.SharedTypeEntity<Dictionary<string, object>>("GenreMovie")
                .HasData(
                new { GenresId = 1, MoviesId = 1},
                new { GenresId = 1, MoviesId = 2 },
                new { GenresId = 1, MoviesId = 3 },
                new { GenresId = 2, MoviesId = 1 });

            modelBuilder.SharedTypeEntity<Dictionary<string, object>>("ActorMovie")
                .HasData(
                new { MoviesId = 1, ActorsId = 1});
        }

        public DbSet<Movie> Movies => Set<Movie>();
        public DbSet<Actor> Actors => Set<Actor>();
        public DbSet<Genre> Genres => Set<Genre>();
    }
}
