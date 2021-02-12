using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Cart.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ECommerce.Cart.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("{customerId:int}")]
        public async Task<IActionResult> Get(int customerId)
        {
            var cartItems = await _cartService.GetCart(customerId);

            return Ok(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CartDto model)
        {
            await _cartService.AddToCart(model);

            return Ok();
        }
    }
}