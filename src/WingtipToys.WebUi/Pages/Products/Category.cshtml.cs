using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WingtipToys.WebUi.Pages.Products
{
    public class CategoryModel : PageModel
    {

        private readonly Domain.IProductManager _productManager;

        public CategoryModel(Domain.IProductManager productManager)
        {
            _productManager = productManager;
        }

        public void OnGet(int? id)
        {

            if (id.HasValue)
            {
                Products = _productManager.GetProductsByCategoryId(id.Value);
                var category = _productManager.GetCategoryById(id.Value);
                CategoryName = category != null ? category.CategoryName : "Category not Found";
            }
            else
            {
                Products = _productManager.GetProducts();
                CategoryName = "All Categories";

            }
        }

        public List<Domain.Models.Product> Products { get; set; }

        public string CategoryName { get; set; }

    }
}