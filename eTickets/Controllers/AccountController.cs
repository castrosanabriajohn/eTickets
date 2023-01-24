using eTickets.Data;
using eTickets.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace eTickets.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager; // For user related data
        private readonly SignInManager<ApplicationUser> _signInManager; // To check if user is signed in
        private readonly AppDbContext _context;
        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 AppDbContext context) => (_userManager, _signInManager, _context) = (userManager, signInManager, context);
        public IActionResult Index()
        {
            return View();
        }
    }
}
