using System.Threading.Tasks;
using ECommerce.UI.Models;
using ECommerce.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ECommerce.UI.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly ILogger<CartController> _logger;

        public CartController(ICartService cartService, ILogger<CartController> logger)
        {
            _cartService = cartService;
            _logger = logger;
        }

        public async Task<IActionResult> Add(int id)
        {
            _logger.LogDebug("Adding product to cart. ProductId: {ProductId}", id);

            await _cartService.AddToCart(new CartDto {CustomerId = 1, Quantity = 1, ProductId = id});

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogDebug("Getting cart details.");

            var cartDetails = await _cartService.GetCartDetails(1);

            _logger.LogDebug("Found {CartProductCount} product(s) in cart.", cartDetails.Count);

            return View(cartDetails);
        }
    }
}