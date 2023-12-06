using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BridgetSandalsAPI.Models
{
    public class ProductVariant
    {
        [Key]
        public int Id { get; set; }


        [Column(TypeName = "varchar(20)")]
        public string Color { get; set; }



        [Column(TypeName = "varchar(10)")]
        public string Size { get; set; }



        [Column(TypeName = "decimal(8,2)")]
        public decimal Price { get; set; }


        public int Quantity { get; set; }


        // Foreign key to Product
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
