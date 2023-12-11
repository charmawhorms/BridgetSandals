using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace BridgetSandalsClient.Models
{
    public class ShoppingCart
    {
        [DisplayName("Product Id")]
        public int ProductId { get; set; }


        [DisplayName("Image")]
        public string ProductImageFilePath { get; set; }


        [DisplayName("Product Name")]
        public string ProductName { get; set; }


        public decimal Price { get; set; }


        public int Quantity { get; set; }
    }
}
