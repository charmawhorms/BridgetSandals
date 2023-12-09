using BridgetSandalsClient.Models;
using BridgetSandalsClient.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace BridgetSandalsClient.Controllers
{
    public class CartController : Controller
    {
        // Display the cart
        public IActionResult Index()
        {
            var cart = GetCart();
            return View(cart);
        }



        // Get the cart from session or create a new one
        private List<ShoppingCart> GetCart()
        {
            var cart = HttpContext.Session.Get<List<ShoppingCart>>("Cart");
            return cart ?? new List<ShoppingCart>();
        }


        // Save the cart to session
        private void SaveCart(List<ShoppingCart> cart)
        {
            HttpContext.Session.Set("Cart", cart);
        }


        // Add an item to the cart
        public IActionResult AddToCart(int productId, string productName, decimal price, int quantity = 1)
        {
            var cart = GetCart();

            // Check if the product is already in the cart, if so, update the quantity
            var existingItem = cart.FirstOrDefault(item => item.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                // If the product is not in the cart, add it
                var newItem = new ShoppingCart
                {
                    ProductId = productId,
                    ProductName = productName,
                    Price = price,
                    Quantity = quantity
                };
                cart.Add(newItem);
            }

            SaveCart(cart);

            return RedirectToAction("Index", "Home"); // Redirect to a different action
        }


        // Remove an item from the cart
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = GetCart();
            var itemToRemove = cart.FirstOrDefault(item => item.ProductId == productId);

            if (itemToRemove != null)
            {
                cart.Remove(itemToRemove);
                SaveCart(cart);
            }

            return RedirectToAction("Index", "Cart");
        }


        public int GetCartItemCount()
        {
            var cart = HttpContext.Session.Get<List<ShoppingCart>>("Cart");
            int itemCount = cart?.Count ?? 0; // If cart is null, set itemCount to 0; otherwise, count the items

            return itemCount;
        }
    }
}
