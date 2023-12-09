using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BridgetSandalsAPI.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set; } // Collection of items in the cart
        public Cart()
        {
            CartItems = new List<CartItem>(); // Initialize the CartItems collection
        }
        public decimal TotalPrice
        {
            get
            {
                // Check if CartItems is null or empty before calculating the total price
                if (CartItems == null || !CartItems.Any())
                {
                    return 0; // Return 0 or another default value as appropriate
                }

                // Calculate the total price based on the items in the cart
                return CartItems.Sum(item => item.Quantity * item.Product.Price);
            }
        }

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
    }
}
