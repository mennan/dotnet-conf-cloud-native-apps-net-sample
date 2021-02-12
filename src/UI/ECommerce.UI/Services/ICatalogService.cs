using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.UI.Models;

namespace ECommerce.UI.Services
{
    public interface ICatalogService
    {
        Task<ProductDto> GetProductById(int productId);
        Task<List<ProductDto>> GetProducts();
    }
}