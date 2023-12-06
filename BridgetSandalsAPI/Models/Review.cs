using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BridgetSandalsAPI.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }


        [Required]
        public int CustomerId { get; set; }


        [Required]
        public int ProductId { get; set; }


        [Required]
        [Range(1, 5)]
        [Column(TypeName = "int")]
        public int Ratings { get; set; }


        [Required]
        [Column(TypeName = "varchar(320)")]
        public string Description { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.Now;


        //Relationship
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }


        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}
