using eTickets.Data.Services;
using eTickets.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace eTickets.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMoviesService _service;
        public MoviesController(IMoviesService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index()
        {
            // Add include property
            var allMovies = await _service.GetAllAsync(m => m.Cinema);
            return View(allMovies);
        }
        // GET: Movies/Details/Id
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
    }
}
