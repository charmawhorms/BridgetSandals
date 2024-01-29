using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BridgetSandalsClient.Models
{
    public class Order
    {
        public int Id { get; set; }

        [DisplayName("Customer Name")]
        [Required(ErrorMessage = "Please enter your name")]
        public string CustomerName { get; set; }

        [DisplayName("Email")]
        [Required(ErrorMessage = "Please enter your email")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        [DisplayName("Phone Number")]
        [Required(ErrorMessage = "Please enter your phone number")]
        [Phone(ErrorMessage = "Please enter a valid phone number")]
        public string PhoneNumber { get; set; }

        [DisplayName("Address")]
        [Required(ErrorMessage = "Please enter your address")]
        public string Address { get; set; }

        // Payment Information
        [DisplayName("Card Number")]
        [Required(ErrorMessage = "Please enter your card number")]
        public string CardNumber { get; set; }

        [DisplayName("Expiration Date")]
        [Required(ErrorMessage = "Please enter the expiration date")]
        public DateTime ExpiryDate { get; set; }

        [DisplayName("CVV")]
        [Required(ErrorMessage = "Please enter the CVV")]
        public string CVV { get; set; }

        [DisplayName("Name on the Card")]
        [Required(ErrorMessage = "Please enter the name on the card")]
        public string NameOnCard { get; set; }

        public virtual List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
