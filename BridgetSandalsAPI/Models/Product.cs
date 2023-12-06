using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BridgetSandalsAPI.Models
{
    public class Product
    {
        public int Id { get; set; }


        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Name { get; set; }


        [Display(Name = "Short Description")]
        [Column(TypeName = "varchar(50)")]
        [MaxLength(50)]
        public string ShortDescription { get; set; }

        public string Description { get; set; }


        [Required]
        [Column(TypeName = "decimal(8,2)")]
        public decimal Price { get; set; }


        [Required]
        public int Quantity { get; set; }
        

        public int? CategoryId { get; set; }


        //Product Image File Path
        public string? ProductImageFilePath { get; set; } = String.Empty;


        //Relationships
        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }


        public virtual ICollection<ProductVariant>? Variants { get; set; }

        //public virtual ICollection<ProductDiscount>? ProductDiscounts { get; set; }

    }
}
