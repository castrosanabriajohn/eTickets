using eTickets.Data;
using eTickets.Data.ViewModels;
using eTickets.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

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
        public IActionResult Login() => View(model: new LoginVM());
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!@ModelState.IsValid) return View(model: loginVM);
            ApplicationUser user = await _userManager.FindByEmailAsync(email: loginVM.EmailAddress);
            if (user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user: user, password: loginVM.Password);
                if (passwordCheck)
                {
                    SignInResult result = await _signInManager.PasswordSignInAsync(user: user, password: loginVM.Password, isPersistent: false, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(actionName: "Index", controllerName: "Movies");
                    }
                    TempData["Error"] = "Wrong credentials, please try again!";
                    return View(model: loginVM);
                }
            }
            TempData["Error"] = "Wrong credentials, please try again!";
            return View(model: loginVM);
        }
    }
}
