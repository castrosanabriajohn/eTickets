using eTickets.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace eTickets.Controllers
{
    public class ActorsController : Controller
    {
        // Declare the app Db Context
        private readonly AppDbContext _context;
        // Inject the context in the constructor
        public ActorsController(AppDbContext context)
        {
            _context = context;
        }
        // Default Method/ActionResult -> Index() was created
        // To call this view go to -> Actors/Index
        public IActionResult Index()
        {
            var data = _context.Actors.ToList();
            return View();
        }
    }
}
