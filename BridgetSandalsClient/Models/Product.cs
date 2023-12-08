using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System.ComponentModel;

namespace BridgetSandalsClient.Models
{
    public class Product
    {
        public int Id { get; set; }


        public string Name { get; set; }


        [DisplayName("Short Description")]
        public string ShortDescription { get; set; }


        public string Description { get; set; }


        public decimal Price { get; set; }


        public int Quantity { get; set; }


        [DisplayName("Category")]
        public int CategoryId { get; set; }


        public string? ProductImageFilePath { get; set; } = String.Empty;


        //Navigational Properties
        public Category? Category { get; set; }


        //public ICollection<ProductVariant>? Variants { get; set; }
    }
}
