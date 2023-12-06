using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BridgetSandalsAPI.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }


        [Required]
        [Column(TypeName = "int")]
        public int OrderId { get; set; }


        [Required]
        [Column(TypeName = "int")]
        public int ProductId { get; set; }


        [Required]
        [Column(TypeName = "int")]
        public int Quantity { get; set; }


        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }


        //Relationship
        public virtual Order? Order { get; set; } // Reference to the order

        public virtual Product? Product { get; set; } // Reference to the product

    }
}
