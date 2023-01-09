using eTickets.Data.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eTickets.Controllers
{
    public class ActorsController : Controller
    {
        // Declare the ActorsService interface
        private readonly IActorsService _service;
        // Inject the service in the constructor
        public ActorsController(IActorsService service)
        {
            _service = service;
        }
        // Default Method/ActionResult -> Index() was created
        // To call this view go to -> Actors/Index
        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAll();
            // Pass the actors data from the controller to the view
            return View(data);
        }
    }
}
