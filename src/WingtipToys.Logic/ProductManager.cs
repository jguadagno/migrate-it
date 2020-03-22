using System;
using System.Collections.Generic;
using System.Linq;
using WingtipToys.Data;
using WingtipToys.Domain;
using WingtipToys.Domain.Models;

namespace WingtipToys.Logic
{
    public class ProductManager : IProductManager
    {
        private readonly ProductContext _db;

        public ProductManager(ProductContext productContext)
        {
            _db = productContext;
        }

        public bool AddProduct(string ProductName, string ProductDesc, string ProductPrice, string ProductCategory, string ProductImagePath)
        {
            var myProduct = new Product();
            myProduct.ProductName = ProductName;
            myProduct.Description = ProductDesc;
            myProduct.UnitPrice = Convert.ToDouble(ProductPrice);
            myProduct.ImagePath = ProductImagePath;
            myProduct.CategoryID = Convert.ToInt32(ProductCategory);

            // Add product to DB.
            _db.Products.Add(myProduct);
            return _db.SaveChanges() != 0;
        }

        public List<Category> GetCategories()
        {
            return _db.Categories.ToList();
        }

        public Category GetCategoryById(int id)
        {
            return _db.Categories.FirstOrDefault(c => c.CategoryID == id);
        }

        public Product GetProductById(int productId)
        {
            return _db.Products.FirstOrDefault(p => p.ProductID == productId);
        }

        public Product GetProductByName(string productName)
        {
            return _db.Products.FirstOrDefault(p => p.ProductName == productName);
        }

        public List<Product> GetProducts()
        {
            return _db.Products.ToList();
        }

        public List<Product> GetProductsByCategoryId(int categoryId)
        {
            return _db.Products.Where(p => p.CategoryID == categoryId).ToList();
        }

        public List<Product> GetProductsByCategoryName(string categoryName)
        {
            return _db.Products.Where(p => p.Category.CategoryName == categoryName).ToList();
        }

        public bool DeleteProduct(int productId)
        {
            var product = _db.Products.FirstOrDefault(p => p.ProductID == productId);
            if (product == null)
            {
                return false;
            }

            _db.Products.Remove(product);
            return _db.SaveChanges() != 0;
        }
    }
}
