using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BridgetSandalsAPI.Models
{
    public class Discount
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(0, 100)]
        [Column(TypeName = "decimal(4,2)")]
        public decimal DiscountPercent { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public bool IsActive { get; set;}


        // Relationships
        public virtual ICollection<ProductDiscount> ProductDiscounts { get; set; }

    }
}
