using System.ComponentModel;

namespace BridgetSandalsClient.Models
{
    public class Category
    {
        public int Id { get; set; }


        [DisplayName("Category Name")]
        public string Name { get; set; }


        public string Description { get; set; }
    }
}
