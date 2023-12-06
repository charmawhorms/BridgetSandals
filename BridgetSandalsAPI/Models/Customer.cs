using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BridgetSandalsAPI.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }


        [Required]
        [Column(TypeName = "varchar(100)")]
        public string FirstName { get; set; }


        [Required]
        [Column(TypeName = "varchar(100)")]
        public string LastName { get; set; }


        [Required]
        [Column(TypeName = "varchar(255)")]
        public string Address1 { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string Address2 { get; set; }


        [Required]
        [Column(TypeName = "int")]
        public int ParishId { get; set; }


        [Required]
        [Column(TypeName = "varchar(13)")]
        public string PhoneNumber { get; set; }


        [Required]
        [Column(TypeName = "varchar(320)")]
        public string Email { get; set; }


        //Look back on this for security reasons - password hash
        [Required]
        public string Password { get; set; }


        //Relationship
        [ForeignKey("ParishId")]
        public virtual Parish? Parish { get; set; }
    }
}
