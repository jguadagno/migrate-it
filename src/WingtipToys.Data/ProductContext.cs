using Microsoft.EntityFrameworkCore;
using WingtipToys.Domain.Models;

namespace WingtipToys.Data
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options): base(options){ }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (false)
            {
                var database = new ProductDatabaseInitializer();
                database.Initialize(this);
                database.Seed(this);
            }
        }
    }
}