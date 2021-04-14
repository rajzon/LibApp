using System.Threading.Tasks;
using Identity.API.Models;
using Identity.API.Models.AuthViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public AuthController(
            SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            var user = await _userManager.FindByEmailAsync(vm.Email);

            var result = await _signInManager.PasswordSignInAsync(user, vm.Password, false, false);

            var userCtx = HttpContext.User;
            if (result.Succeeded)
            {
                return Ok("Logged in");
            }

            return View();
        }
    }
}