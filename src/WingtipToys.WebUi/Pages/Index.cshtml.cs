using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WingtipToys.WebUi.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly Domain.IProductManager _productManager;

        public IndexModel(ILogger<IndexModel> logger, Domain.IProductManager productManager)
        {
            _logger = logger;
            _productManager = productManager;

            var categories = _productManager.GetCategories();
            logger.LogDebug(categories.Count().ToString());
        }

        public void OnGet()
        {

        }
    }
}
