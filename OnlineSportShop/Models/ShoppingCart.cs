using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineSportShop.Models
{
    public class ShoppingCart
    {
        [Key]
        public int CartId { get; set; }
        public int ProId { get; set; }
        [ForeignKey("ProId")]
        public virtual Product Product { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity can't be less than 1")]
        public int Qty { get; set; }

        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser? ApplicationUser { get; set; }
    }
}
