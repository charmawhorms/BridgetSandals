using System.ComponentModel.DataAnnotations;

namespace BridgetSandalsAPI.Models
{
    public class Parish
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
