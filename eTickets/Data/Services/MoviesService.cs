using eTickets.Data.Base;
using eTickets.Data.ViewModels;
using eTickets.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTickets.Data.Services
{
    public class MoviesService : EntityBaseRepository<Movie>, IMoviesService
    {
        private readonly AppDbContext _context;
        public MoviesService(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task AddMovieAsync(NewMovieVM newMovie)
        {
            Movie movie = new()
            {
                Name = newMovie.Name,
                Description = newMovie.Description,
                Price = newMovie.Price,
                ImageURL = newMovie.ImageURL,
                CinemaId = newMovie.CinemaId,
                StartDate = newMovie.StartDate,
                EndDate = newMovie.EndDate,
                MovieCategory = newMovie.MovieCategory,
                ProducerId = newMovie.ProducerId,
            };
            await _context.Movies
                .AddAsync(movie);
            await _context.SaveChangesAsync();
            // Add movie actors
            foreach (var actorId in newMovie.ActorIds)
            {
                Actor_Movie actorMovie = new()
                {
                    MovieId = movie.Id,
                    ActorId = actorId,
                };
                await _context.Actors_Movies
                    .AddAsync(actorMovie);
            }
            await _context.SaveChangesAsync();
        }
        public async Task<Movie> GetMovieByIdAsync(int id)
        {
            var movieDetails = await _context.Movies
                .Include(c => c.Cinema)
                .Include(p => p.Producer)
                .Include(am => am.Actors_Movies).ThenInclude(a => a.Actor)
                .FirstOrDefaultAsync(m => m.Id == id);
            return movieDetails;
        }
        public async Task<NewMovieDropdownsVM> GetNewMovieDropdownsValues()
        {
            NewMovieDropdownsVM response = new()
            {
                Actors = await _context.Actors.
                OrderBy(a => a.FullName).
                ToListAsync(),
                Cinemas = await _context.Cinemas
                .OrderBy(c => c.Name)
                .ToListAsync(),
                Producers = await _context.Producers
                .OrderBy(p => p.FullName)
                .ToListAsync(),
            };
            return response;
        }
        public async Task UpdateMovieAsync(NewMovieVM newMovie)
        {
            Movie dbMovie = await _context.Movies.FirstOrDefaultAsync(n => n.Id == newMovie.Id);
            if (dbMovie != null)
            {
                dbMovie.Name = newMovie.Name;
                dbMovie.Description = newMovie.Description;
                dbMovie.Price = newMovie.Price;
                dbMovie.ImageURL = newMovie.ImageURL;
                dbMovie.CinemaId = newMovie.CinemaId;
                dbMovie.StartDate = newMovie.StartDate;
                dbMovie.EndDate = newMovie.EndDate;
                dbMovie.MovieCategory = newMovie.MovieCategory;
                dbMovie.ProducerId = newMovie.ProducerId;
                await _context.SaveChangesAsync();
            }
            //Remove existing actors
            List<Actor_Movie> existingActorsDb = _context.Actors_Movies.Where(am => am.MovieId == newMovie.Id).ToList();
            _context.Actors_Movies
                .RemoveRange(existingActorsDb);
            await _context.SaveChangesAsync();
            //Add Movie Actors
            foreach (var actorId in newMovie.ActorIds)
            {
                Actor_Movie actorMovie = new()
                {
                    MovieId = newMovie.Id,
                    ActorId = actorId,
                };
                await _context.Actors_Movies
                    .AddAsync(actorMovie);
            }
            await _context.SaveChangesAsync();
        }
    }
}
