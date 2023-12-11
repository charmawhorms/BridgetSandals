namespace BridgetSandalsClient.Models.ViewModel
{
    public class ShoppingCartVM
    {
        public List<ShoppingCart> CartItems { get; set; }
        public int CartCount => CartItems?.Count ?? 0;
    }
}
