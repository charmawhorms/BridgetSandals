namespace BridgetSandalsAPI.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        public int CartId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }


        //Relationship
        public virtual Cart Cart { get; set; } // Reference to the cart
        public virtual Product Product { get; set; } // Reference to the product
    }
}
