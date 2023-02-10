using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineSportShop.Models;

namespace OnlineSportShop.Data
{
    public class OnlineSportShopContext :IdentityDbContext<ApplicationUser>
    {
        public OnlineSportShopContext(DbContextOptions<OnlineSportShopContext> options)
           :base(options) 
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }

    }
}
