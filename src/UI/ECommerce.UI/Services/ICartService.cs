using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.UI.Models;

namespace ECommerce.UI.Services
{
    public interface ICartService
    {
        Task AddToCart(CartDto model);
        Task<List<CartDetailsDto>> GetCartDetails(int customerId);
    }
}