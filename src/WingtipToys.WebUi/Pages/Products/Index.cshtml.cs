using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WingtipToys.WebUi.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly Domain.IProductManager _productManager;

        public IndexModel(Domain.IProductManager productManager)
        {
            _productManager = productManager;
        }
        public void OnGet()
        {
            Products = _productManager.GetProducts();
        }
        public List<Domain.Models.Product> Products { get; set; }

    }
}