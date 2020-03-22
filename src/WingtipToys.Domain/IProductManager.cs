using System.Collections.Generic;
using WingtipToys.Domain.Models;

namespace WingtipToys.Domain
{
    public interface IProductManager
    {
        bool AddProduct(string ProductName, string ProductDesc, string ProductPrice, string ProductCategory, string ProductImagePath);
        bool DeleteProduct(int productId);
        List<Category> GetCategories();
        Category GetCategoryById(int categoryId);
        Product GetProductById(int productId);
        Product GetProductByName(string productName);
        List<Product> GetProducts();
        List<Product> GetProductsByCategoryId(int categoryId);
        List<Product> GetProductsByCategoryName(string categoryName);
    }
}