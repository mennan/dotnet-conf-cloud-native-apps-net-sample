using System.Threading.Tasks;
using ECommerce.Cart.Models;

namespace ECommerce.Cart
{
    public interface ICatalogService
    {
        Task<ProductDto> GetProductById(int productId);
    }
}