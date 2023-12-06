using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BridgetSandalsClient.Models
{
    public class Review
    {
        public int Id { get; set; }


        [DisplayName("Customer")]
        public int CustomerId { get; set; }


        [DisplayName("Product Name")]
        [Required]
        public int ProductId { get; set; }


        public int Ratings { get; set; }


        public string Description { get; set; }


        [DisplayName("Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;


        //Navigational Properties
        public Customer Customer { get; set; }


        public Product Product { get; set; }
    }
}
