using System.Threading.Tasks;
using ECommerce.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ECommerce.UI.Controllers
{
    public class ProductController : Controller
    {
        private readonly ICatalogService _catalogService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(ICatalogService catalogService, ILogger<ProductController> logger)
        {
            _catalogService = catalogService;
            _logger = logger;
        }

        public async Task<IActionResult> Detail(int id)
        {
            _logger.LogDebug("Getting product details. ProductId: {ProductId}", id);

            var product = await _catalogService.GetProductById(id);

            if (product == null)
            {
                _logger.LogWarning("Product not found! ProductId: {ProductId}", id);
                return NotFound();
            }

            _logger.LogDebug("Product found. ProductId: {ProductId}, Name: {ProductName}", id, product.Name);

            return View(product);
        }
    }
}