using eTickets.Data;
using eTickets.Data.Static;
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
                bool passwordCheck = await _userManager.CheckPasswordAsync(user: user, password: loginVM.Password);
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
        public IActionResult Register() => View(model: new RegisterVM());
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View(registerVM);
            ApplicationUser applicationUser = await _userManager.FindByEmailAsync(email: registerVM.EmailAddress);
            if (applicationUser != null)
            {
                TempData["Error"] = "This email address is already in use.";
                return View(model: registerVM);
            }
            ApplicationUser newUser = new()
            {
                FullName = registerVM.FullName,
                Email = registerVM.EmailAddress,
                UserName = registerVM.EmailAddress,
            };
            IdentityResult identityResult = await _userManager.CreateAsync(user: newUser, password: registerVM.Password);
            if (identityResult.Succeeded)
                await _userManager.AddToRoleAsync(user: newUser, role: UserRoles.User);
            return View(viewName: "RegisterCompleted");
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(actionName: "Index", controllerName: "Movies");
        }
    }
}
