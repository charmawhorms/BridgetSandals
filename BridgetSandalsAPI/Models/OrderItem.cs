using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BridgetSandalsAPI.Models
{
    public class OrderItem
    {
        [Key] 
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string ProductImageFilePath { get; set; }

        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }
}
