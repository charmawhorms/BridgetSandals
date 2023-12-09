using System.ComponentModel.DataAnnotations.Schema;

namespace BridgetSandalsClient.Models
{
    public class ShoppingCart
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
