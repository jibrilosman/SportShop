using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineSportShop.Models
{
    public class Category
    {
        [Key]
        public int CatId { get; set; }
        [Required(ErrorMessage = "Category name required")]
        public string? CatName { get; set; }
        public string? CatPhoto { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }

        public virtual ICollection<Product> Product { get; set; }
    }
}
