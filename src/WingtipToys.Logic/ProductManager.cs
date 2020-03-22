using System;
using System.Collections.Generic;
using System.Linq;
using WingtipToys.Data;

namespace WingtipToys.Logic
{
    public class ProductManager
    {
        public bool AddProduct(string ProductName, string ProductDesc, string ProductPrice, string ProductCategory, string ProductImagePath)
        {
            var myProduct = new WingtipToys.Domain.Models.Product();
            myProduct.ProductName = ProductName;
            myProduct.Description = ProductDesc;
            myProduct.UnitPrice = Convert.ToDouble(ProductPrice);
            myProduct.ImagePath = ProductImagePath;
            myProduct.CategoryID = Convert.ToInt32(ProductCategory);

            using (ProductContext _db = new ProductContext())
            {
                // Add product to DB.
                _db.Products.Add(myProduct);
                _db.SaveChanges();
            }
            // Success.
            return true;
        }

        public List<Domain.Models.Category> GetCategories()
        {
            using (ProductContext _db = new ProductContext())
            {
                return _db.Categories.ToList();
            }
        }
    }
}
