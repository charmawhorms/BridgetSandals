using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace BridgetSandalsClient.Models
{
    public class Customer
    {
        public int Id { get; set; }


        [DisplayName("First Name")]
        public string FirstName { get; set; }


        [DisplayName("Last Name")]
        public string LastName { get; set; }


        [DisplayName("Address 1")]
        public string Address1 { get; set; }


        [DisplayName("Address 2")]
        public string Address2 { get; set; }


        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }


        public string Email { get; set; }


        //Look back on this for security reasons - password hash
        [Required]
        public string Password { get; set; }


        //Dropdown list for Parish
        public List<SelectListItem>? ParishList { get; set; }


        //Selected Parish
        [DisplayName("Parish")]
        public int SelectedParishId { get; set; }
    }
}
