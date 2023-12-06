using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace BridgetSandalsClient.Models.ViewModel
{
    public class ProductVM
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ShortDescription { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public IFormFile? ProductImageFile { get; set; }


        public virtual ICollection<ProductVariant> Variants { get; set; }


        //Dropdown list for Category
        public List<SelectListItem>? CategoryList { get; set; }


        //Selected Category
        [DisplayName("Category")]
        public int SelectedCategoryId { get; set; }
    }
}
