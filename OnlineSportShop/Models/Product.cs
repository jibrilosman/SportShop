using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineSportShop.Models
{
    public class Product
    {
        [Key]
        public int ProId { get; set; }
        [Required(ErrorMessage = "Product name required")]
        public string? ProName { get; set; }
        [Required(ErrorMessage = "Product description required")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Product price required")]
        public decimal Price { get; set; }
        public string? ProImage { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }

        public int CatId { get; set; }
        [ForeignKey("CatId")]
        public virtual Category Category { get; set; }
    }
}
