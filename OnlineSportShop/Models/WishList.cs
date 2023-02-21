using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineSportShop.Models
{
    public class WishList
    {
        [Key]
        public int? WishId { get; set; }
        public int ProId { get; set; }
        [ForeignKey("ProId")]
        public virtual Product Product { get; set; }
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser? ApplicationUser { get; set; }
    }
}
