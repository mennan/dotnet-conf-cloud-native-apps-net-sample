using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Cart.Cache;
using ECommerce.Cart.Models;
using Microsoft.Extensions.Logging;

namespace ECommerce.Cart
{
    public class CartService : ICartService
    {
        private readonly ICacheProvider _cache;
        private readonly ICatalogService _catalogService;
        private readonly ILogger<CartService> _logger;
        private const string KeyPattern = "cart_{0}";

        public CartService(ICacheProvider cache, ICatalogService catalogService, ILogger<CartService> logger)
        {
            _cache = cache;
            _catalogService = catalogService;
            _logger = logger;
        }

        public async Task AddToCart(CartDto cart)
        {
            try
            {
                var key = string.Format(KeyPattern, cart.CustomerId);
                var cartData = await _cache.GetAsync<List<CartDetailsDto>>(key) ?? new List<CartDetailsDto>();
                var product = await _catalogService.GetProductById(cart.ProductId);
                var price = product?.Price ?? 0;

                cartData.Add(new CartDetailsDto
                {
                    ProductId = cart.ProductId,
                    Quantity = cart.Quantity,
                    Price = price * cart.Quantity
                });

                await _cache.SetAsync(key, cartData, (int) TimeSpan.FromDays(1).TotalSeconds);
            }
            catch (Exception ex)
            {
                _logger.LogError("AddToCart Error", ex);
            }
        }

        public async Task<List<CartDetailsDto>> GetCart(int customerId)
        {
            var key = string.Format(KeyPattern, customerId);
            var cartData = await _cache.GetAsync<List<CartDetailsDto>>(key);

            if (cartData != null)
            {
                _logger.LogDebug("Found {ProductCount} product(s) in cart.", cartData.Count);

                return cartData.Select(x => new CartDetailsDto
                {
                    ProductId = x.ProductId,
                    Name = GetProductName(x.ProductId),
                    Price = x.Price,
                    Quantity = x.Quantity
                }).ToList();
            }

            _logger.LogDebug("Cart is empty.");

            return new List<CartDetailsDto>();
        }

        private string GetProductName(int productId)
        {
            var product = _catalogService.GetProductById(productId).GetAwaiter().GetResult();

            return product?.Name ?? string.Empty;
        }
    }
}