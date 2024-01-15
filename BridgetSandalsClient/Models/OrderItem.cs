using System.ComponentModel.DataAnnotations.Schema;

namespace BridgetSandalsClient.Models
{
    [Serializable]
    public class OrderItem
    {
        public int ProductId { get; set; }

        public string ProductImageFilePath { get; set; }

        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }
}
