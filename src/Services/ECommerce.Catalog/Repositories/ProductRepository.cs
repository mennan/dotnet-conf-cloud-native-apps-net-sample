using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Catalog.Contexts;
using ECommerce.Catalog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.Catalog.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly CatalogDbContext _dbContext;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(CatalogDbContext dbContext, ILogger<ProductRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<List<Product>> GetProducts()
        {
            try
            {
                var products = await _dbContext.Products.ToListAsync();

                _logger.LogDebug("Found {ProductCount} product(s).", products.Count);

                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occurred on Product repository.");
                throw;
            }
        }

        public async Task<Product> GetProductById(int id)
        {
            try
            {
                var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);

                if(product == null) _logger.LogWarning("The {ProductId} product was not found in the database.", id);
                
                _logger.LogInformation("The {ProductId} product was found in the database.", id);
                
                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occurred on Product repository.");
                throw;
            }
        }
    }
}