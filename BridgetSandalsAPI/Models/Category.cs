using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BridgetSandalsAPI.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }


        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; }


        [Required]
        [Column(TypeName = "varchar(255)")]
        public string Description { get; set; }
    }
}
