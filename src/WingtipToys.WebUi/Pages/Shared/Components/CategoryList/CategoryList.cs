using Microsoft.AspNetCore.Mvc;

namespace WingtipToys.WebUi.Pages.Shared.Components.CategoryList
{
    public class CategoryList : ViewComponent
    {
        private readonly Domain.IProductManager _productManager;

        public CategoryList(Domain.IProductManager productManager)
        {
            _productManager = productManager;
        }

        public IViewComponentResult Invoke()
        {
            var categories = _productManager.GetCategories();
            return View(categories);
        }
    }
}
