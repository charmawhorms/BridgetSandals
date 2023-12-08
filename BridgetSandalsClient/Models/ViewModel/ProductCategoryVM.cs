namespace BridgetSandalsClient.Models.ViewModel
{
    public class ProductCategoryVM
    {
        public List<Product> Products { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
