using eTickets.Data.Services;
using eTickets.Data.Static;
using eTickets.Data.ViewModels;
using eTickets.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTickets.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class MoviesController : Controller
    {
        private readonly IMoviesService _service;
        public MoviesController(IMoviesService service)
        {
            _service = service;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            // Add include property
            var allMovies = await _service.GetAllAsync(m => m.Cinema);
            return View(allMovies);
        }
        [AllowAnonymous]
        public async Task<IActionResult> Filter(string searchString)
        {
            // Add include property
            List<Movie> allMovies = (List<Movie>)await _service.GetAllAsync(includeProperties: m => m.Cinema);
            if (!string.IsNullOrEmpty(searchString))
            {
                List<Movie> filteredMovies = allMovies.Where(predicate: am => am.Name.Contains(searchString)
                || am.Description.Contains(searchString)).ToList();
                return View(nameof(Index), filteredMovies);
            }
            return View(nameof(Index), allMovies);
        }
        // GET: Movies/Details/Id
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var movieDetails = await _service.GetMovieByIdAsync(id);
            return View(movieDetails);
        }
        // GET: Movies/Create
        public async Task<IActionResult> Create()
        {
            NewMovieDropdownsVM movieDropdownsValues = await _service.GetNewMovieDropdownsValues();
            ViewBag.Cinemas = new SelectList(movieDropdownsValues.Cinemas, "Id", "Name");
            ViewBag.Producers = new SelectList(movieDropdownsValues.Producers, "Id", "FullName");
            ViewBag.Actors = new SelectList(movieDropdownsValues.Actors, "Id", "FullName");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(NewMovieVM newMovie)
        {
            if (ModelState.IsValid)
            {
                await _service.AddMovieAsync(newMovie);
                return RedirectToAction(nameof(Index));
            }
            // Reload new movie dropdown values for validation error
            NewMovieDropdownsVM movieDropdownsValues = await _service.GetNewMovieDropdownsValues();
            ViewBag.Cinemas = new SelectList(movieDropdownsValues.Cinemas, "Id", "Name");
            ViewBag.Producers = new SelectList(movieDropdownsValues.Producers, "Id", "FullName");
            ViewBag.Actors = new SelectList(movieDropdownsValues.Actors, "Id", "FullName");
            return View(newMovie);
        }
        // GET: Movies/Edit/Id
        public async Task<IActionResult> Edit(int id)
        {
            Movie movieDetails = await _service.GetMovieByIdAsync(id);
            if (movieDetails != null)
            {
                NewMovieVM response = new()
                {
                    Id = movieDetails.Id,
                    Name = movieDetails.Name,
                    Description = movieDetails.Description,
                    Price = movieDetails.Price,
                    StartDate = movieDetails.StartDate,
                    EndDate = movieDetails.EndDate,
                    ImageURL = movieDetails.ImageURL,
                    MovieCategory = movieDetails.MovieCategory,
                    CinemaId = movieDetails.CinemaId,
                    ProducerId = movieDetails.ProducerId,
                    ActorIds = movieDetails.Actors_Movies.Select(am => am.ActorId).ToList(),
                };
                NewMovieDropdownsVM movieDropdownsValues = await _service.GetNewMovieDropdownsValues();
                ViewBag.Cinemas = new SelectList(movieDropdownsValues.Cinemas, "Id", "Name");
                ViewBag.Producers = new SelectList(movieDropdownsValues.Producers, "Id", "FullName");
                ViewBag.Actors = new SelectList(movieDropdownsValues.Actors, "Id", "FullName");
                return View(response);
            }
            return View("NotFound");
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, NewMovieVM newMovie)
        {
            if (id != newMovie.Id) return View("NotFound");
            if (!ModelState.IsValid)
            {
                NewMovieDropdownsVM movieDropdownsValues = await _service.GetNewMovieDropdownsValues();
                ViewBag.Cinemas = new SelectList(movieDropdownsValues.Cinemas, "Id", "Name");
                ViewBag.Producers = new SelectList(movieDropdownsValues.Producers, "Id", "FullName");
                ViewBag.Actors = new SelectList(movieDropdownsValues.Actors, "Id", "FullName");
                await _service.AddMovieAsync(newMovie);
                return View(newMovie);

            }
            return RedirectToAction(nameof(Index));
        }
    }
}
