namespace ECommerce.Cart.Models
{
    public class CartDto
    {
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}