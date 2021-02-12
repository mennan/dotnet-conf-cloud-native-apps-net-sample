using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.Catalog.Entities;

namespace ECommerce.Catalog.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProducts();
        Task<Product> GetProductById(int id);
    }
}