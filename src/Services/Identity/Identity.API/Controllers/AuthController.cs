using System.Threading.Tasks;
using Identity.API.Models;
using Identity.API.Models.AuthViewModels;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
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

        [HttpGet("logout")]
        public async Task<IActionResult> Logout(string logoutId)
        {
            await _signInManager.SignOutAsync();

            var logoutRequest = await _interactionService.GetLogoutContextAsync(logoutId);

            return Redirect(logoutRequest.PostLogoutRedirectUri);
        }
        
        [HttpGet("login")]
        public IActionResult Login(string returnUrl)
        {
            returnUrl ??= "http://localhost:4200";
            
            return View(new LoginViewModel {ReturnUrl = returnUrl});
        }
        //Name loginform
        [HttpPost("login-form", Name = "login-form")]
        public async Task<IActionResult> Login([FromForm] LoginViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.IsUserPassedIncorrectCredentials = true;
                return View(vm);
            }
            

            var user = await _userManager.FindByEmailAsync(vm.Email);
            if (user is null)
            {
                vm.IsUserPassedIncorrectCredentials = true;
                return View(vm);
            }


            var result = await _signInManager.PasswordSignInAsync(user, vm.Password, false, false);

            var userCtx = HttpContext.User;
            if (result.Succeeded)
            {
                return Redirect(vm.ReturnUrl);
            }

            vm.IsUserPassedIncorrectCredentials = true;
            return View(vm);


        }
    }
}