using System.ComponentModel.DataAnnotations;

namespace BridgetSandalsAPI.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set; } // Collection of items in the cart

        public decimal TotalPrice
        {
            get
            {
                // Calculate the total price based on the items in the cart
                return CartItems.Sum(item => item.Quantity * item.Product.Price);
            }
        }
    }
}
