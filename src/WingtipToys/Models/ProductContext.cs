using System.Data.Entity;
namespace WingtipToys.Models
{
  public class ProductContext : DbContext
  {
    public ProductContext()
      : base("WingtipToys")
    {
    }
    public DbSet<WingtipToys.Domain.Models.Category> Categories { get; set; }
    public DbSet<WingtipToys.Domain.Models.Product> Products { get; set; }
    public DbSet<WingtipToys.Domain.Models.CartItem> ShoppingCartItems { get; set; }
    public DbSet<WingtipToys.Domain.Models.Order> Orders { get; set; }
    public DbSet<WingtipToys.Domain.Models.OrderDetail> OrderDetails { get; set; }
  }
}