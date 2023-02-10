using System.ComponentModel.DataAnnotations;

namespace OnlineSportShop.Models
{
    public class Contact
    {
        [Key]
        public int CoId { get; set; }
        [Required(ErrorMessage = "Name Is required")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress]
        public string? Email { get; set; }
        public string? Subject { get; set; }
        [Required(ErrorMessage = "This field can't be empty")]
        public string? Message { get; set; }
    }
}
