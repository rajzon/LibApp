using System.ComponentModel.DataAnnotations;

namespace Identity.API.Models.AuthViewModels
{
    public class LoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}