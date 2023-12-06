using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BridgetSandalsAPI.Models
{
    public class Order
    {
        [Key] 
        public int Id { get; set; }


        [Required]
        [Column(TypeName = "int")]
        public int CustomerId { get; set; }


        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now;


        public virtual ICollection<OrderItem> OrderItems { get; set; } // Collection of items in the order
        

        public decimal TotalCost
        {
            get
            {
                // Calculate the total cost based on the items in the order
                return OrderItems.Sum(item => item.Quantity * item.UnitPrice);
            }
        }


        [Required]
        [Column(TypeName = "varchar(255)")]
        public string ShippingAddress1 { get; set; } // Shipping address for the order


        [Required]
        [Column(TypeName = "varchar(255)")]
        public string ShippingAddress2 { get; set; }


        [Required]
        [Column(TypeName = "int")]
        public int ShippingAddressParishId { get; set; }


        [Required]
        [Column(TypeName = "varchar(11)")]
        public string PaymentMethod { get; set; }


        [Required]
        [Column(TypeName = "varchar(255)")]
        public string OrderStatus { get; set; } = "Processing";


        //Relationship
        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }


        [ForeignKey("ShippingAddressParishId")]
        public virtual Parish? Parish { get; set; }

    }
}
