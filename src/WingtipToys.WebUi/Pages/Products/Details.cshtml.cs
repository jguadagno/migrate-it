using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WingtipToys.WebUi.Pages.Products
{
    public class DetailsModel : PageModel
    {
        private readonly Domain.IProductManager _productManager;

        public DetailsModel(Domain.IProductManager productManager)
        {
            _productManager = productManager;
        }

        public void OnGet(int? id)
        {
            Product = id.HasValue ? _productManager.GetProductById(id.Value): new Domain.Models.Product();
        }

        public Domain.Models.Product Product { get; set; }
    }
}