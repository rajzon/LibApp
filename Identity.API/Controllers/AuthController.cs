using System.Threading.Tasks;
using Identity.API.Models;
using Identity.API.Models.AuthViewModels;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IIdentityServerInteractionService _interactionService;

        public AuthController(
            SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager, 
            IIdentityServerInteractionService interactionService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _interactionService = interactionService;
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            await _signInManager.SignOutAsync();

            var logoutRequest = await _interactionService.GetLogoutContextAsync(logoutId);

            return Redirect(logoutRequest.PostLogoutRedirectUri);
        }
        
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            returnUrl ??= "http://localhost:4200";
            
            return View(new LoginViewModel {ReturnUrl = returnUrl});
        }
        
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (! ModelState.IsValid)
                return View();
            

            var user = await _userManager.FindByEmailAsync(vm.Email);
            if (user is null)
                return View();


            var result = await _signInManager.PasswordSignInAsync(user, vm.Password, false, false);

            var userCtx = HttpContext.User;
            if (result.Succeeded)
            {
                return Redirect(vm.ReturnUrl);
            }

            return View();


        }
    }
}