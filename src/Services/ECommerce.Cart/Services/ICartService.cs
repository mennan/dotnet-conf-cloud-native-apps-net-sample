using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.Cart.Models;

namespace ECommerce.Cart
{
    public interface ICartService
    {
        Task AddToCart(CartDto cart);
        Task<List<CartDetailsDto>> GetCart(int customerId);
    }
}