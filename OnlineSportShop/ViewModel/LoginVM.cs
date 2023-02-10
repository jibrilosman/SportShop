using System.ComponentModel.DataAnnotations;

namespace OnlineSportShop.ViewModel
{
    public class LoginVM
    {
        [Required(ErrorMessage ="Invalid UserName!!")]
        public string UserName { get; set; }
        [Required(ErrorMessage ="Invalid Password!!")]
        public string Password { get; set; }
    }
}
