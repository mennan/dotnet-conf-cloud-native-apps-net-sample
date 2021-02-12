using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using ECommerce.Cart.Models;
using Microsoft.Extensions.Logging;

namespace ECommerce.Cart
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _client;
        private readonly ILogger<CatalogService> _logger;

        public CatalogService(HttpClient client, ILogger<CatalogService> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<ProductDto> GetProductById(int productId)
        {
            var catalogServiceResponse = await _client.GetAsync($"catalog/{productId}");

            if (!catalogServiceResponse.IsSuccessStatusCode)
            {
                _logger.LogError(
                    "Cannot access to Catalog service. ProductId: {ProductId} Code: {Code} Reason: {Reason}", productId,
                    catalogServiceResponse.StatusCode, catalogServiceResponse.ReasonPhrase);

                throw new ApplicationException($"API call error {catalogServiceResponse.ReasonPhrase}");
            }

            var data = await catalogServiceResponse.Content.ReadAsStringAsync();
            var product = JsonSerializer.Deserialize<ProductDto>(data);

            return product;
        }
    }
}