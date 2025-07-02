using Microsoft.EntityFrameworkCore;
using MovieLibrary.Server.Data;
using MovieLibrary.Server.Interfaces;
using MovieLibrary.Shared;

namespace MovieLibrary.Server.Services
{
    public class ActorService : IActorService
    {
        private readonly AppDataContext _context;

        public ActorService(IDbContextFactory<AppDataContext> context)
        {
            _context = context.CreateDbContext();
        }

        public async Task<List<Actor>> GetActorsAsync()
        {
            return await _context.Actors.ToListAsync();
        }

        public async Task<List<Actor>> GetSortActorsAsync(string order)
        {

            if (order == "name-aorder")
            {
                return await _context.Actors.OrderBy(a => a.Name).ToListAsync();
            }
            else if (order == "name-deorder")
            {
                return await _context.Actors.OrderByDescending(a => a.Name).ToListAsync();
            }
            else if (order == "born-aorder")
            {
                return await _context.Actors.OrderBy(a => a.Born).ToListAsync();
            }
            else if (order == "born-deorder")
            {
                return await _context.Actors.OrderByDescending(a => a.Born).ToListAsync();
            }
            else if (order == "placeofbirth-aorder")
            {
                return await _context.Actors.OrderBy(a => a.PlaceOfBirth).ToListAsync();
            }
            else
            {
                return await _context.Actors.OrderByDescending(a => a.PlaceOfBirth).ToListAsync();
            }
        }

        public async Task<List<Actor>> SearchActorAsync(string condition)
        {
            return await _context.Actors.Where(a => a.Name.ToLower().Contains(condition.ToLower())).ToListAsync();
        }

        public async Task<Actor> GetActorDetailsAsync(int id)
        {
            return await _context.Actors
                .AsSplitQuery().FirstOrDefaultAsync(a => a.Id == id)
                ?? throw new Exception("Actor not found!");
        }

        public async Task<Actor> InsertActorAsync(Actor actor)
        {
            _context.Actors.Add(actor);
            await _context.SaveChangesAsync();
            return actor;
        }

        public async Task UpdateActorAsync(int actorId, Actor actor)
        {
            var item = _context.Attach(actor);
            item.State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Actors.AnyAsync(a => a.Id == actorId))
                {
                    throw new Exception("Actor not found!");
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task DeleteActorAsync(int actorId)
        {
            _context.Actors.Remove(
                new Actor(null!, null!,null!) { Id = actorId });

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Actors.AnyAsync(a => a.Id == actorId))
                    throw new Exception("Actor not found");
                else
                    throw;
            }
        }
    }
}
