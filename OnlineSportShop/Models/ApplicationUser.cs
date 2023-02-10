using Microsoft.AspNetCore.Identity;

namespace OnlineSportShop.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
