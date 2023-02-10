using System.ComponentModel.DataAnnotations;

namespace OnlineSportShop.ViewModel
{
    public class RegistrationVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{6,}$", ErrorMessage = "Minimum length 6 and must contain 1 Uppercase, 1 lowercase, 1 special character and 1 digit")]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }
        public string? Role { get; set; }
    }
}
