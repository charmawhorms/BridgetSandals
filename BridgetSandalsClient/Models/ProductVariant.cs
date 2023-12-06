using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BridgetSandalsClient.Models
{
    public class ProductVariant
    {
        public int Id { get; set; }


        public string Color { get; set; }


        public string Size { get; set; }


        public decimal Price { get; set; }


        public int Quantity { get; set; }


        public int ProductId { get; set; }


        public Product Product { get; set; }
    }
}
