using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ECommerce.UI.Models;
using Microsoft.Extensions.Logging;

namespace ECommerce.UI.Services
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

        public async Task<List<ProductDto>> GetProducts()
        {
            var catalogServiceResponse = await _client.GetAsync($"catalog");

            if (!catalogServiceResponse.IsSuccessStatusCode)
            {
                _logger.LogError(
                    "Cannot access to Catalog service. Code: {Code} Reason: {Reason}",
                    catalogServiceResponse.StatusCode, catalogServiceResponse.ReasonPhrase);

                throw new ApplicationException($"API call error {catalogServiceResponse.ReasonPhrase}");
            }

            var data = await catalogServiceResponse.Content.ReadAsStringAsync();
            var products = JsonSerializer.Deserialize<List<ProductDto>>(data);

            _logger.LogDebug("Found {ProductCount} product(s).", products.Count);

            return products;
        }
    }
}