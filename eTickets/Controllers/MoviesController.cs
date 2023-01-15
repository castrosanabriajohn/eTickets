using eTickets.Data.Services;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Create()
        {
            ViewData["Welcome"] = "Welcome to our Store";
            ViewBag.Description = "Store Description";
            return View();
        }
    }
}
