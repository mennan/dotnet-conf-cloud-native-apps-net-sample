using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ECommerce.UI.Models;
using Microsoft.Extensions.Logging;

namespace ECommerce.UI.Services
{
    public class CartService : ICartService
    {
        private readonly HttpClient _client;
        private readonly ILogger<CartService> _logger;

        public CartService(HttpClient client, ILogger<CartService> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task AddToCart(CartDto model)
        {
            _logger.LogDebug("Adding product to cart. Data: {CartData}", model);
            var postData = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            var cartServiceResponse = await _client.PostAsync("cart", postData);

            if (!cartServiceResponse.IsSuccessStatusCode)
            {
                _logger.LogError(
                    "Cannot access to Cart service. ProductId: {ProductId} Code: {Code} Reason: {Reason}",
                    model.ProductId,
                    cartServiceResponse.StatusCode, cartServiceResponse.ReasonPhrase);

                throw new ApplicationException($"API call error {cartServiceResponse.ReasonPhrase}");
            }
        }

        public async Task<List<CartDetailsDto>> GetCartDetails(int customerId)
        {
            var cartServiceResponse = await _client.GetAsync($"cart/{customerId}");

            if (!cartServiceResponse.IsSuccessStatusCode)
            {
                _logger.LogError(
                    "Cannot access to Cart service. CustomerId: {CustomerId} Code: {Code} Reason: {Reason}", customerId,
                    cartServiceResponse.StatusCode, cartServiceResponse.ReasonPhrase);

                throw new ApplicationException($"API call error {cartServiceResponse.ReasonPhrase}");
            }

            var data = await cartServiceResponse.Content.ReadAsStringAsync();
            var cartDetails = JsonSerializer.Deserialize<List<CartDetailsDto>>(data);

            return cartDetails;
        }
    }
}