using System.ComponentModel.DataAnnotations;

namespace Identity.API.Models.AuthViewModels
{
    public class LoginViewModel
    {
        // [Required(ErrorMessage = "Email address is required")]
        // [EmailAddress(ErrorMessage = "Email address is not valid")]
        public string Email { get; set; }
        // [Required]
        // [StringLength(20, ErrorMessage = "Password must contain at least {2} and at most {1} characters", MinimumLength = 8)]
        public string Password { get; set; }
    }
}