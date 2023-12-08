using BridgetSandalsClient.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace BridgetSandalsClient.Controllers
{
    public class CartController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7027/cart");
        private readonly HttpClient _client;

        public CartController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }


        // Action to view the cart
        [HttpGet]
        public IActionResult ViewCart()
        {
            var response = _client.GetAsync(_client.BaseAddress).Result;
            if (response.IsSuccessStatusCode)
            {
                var cart = response.Content.ReadAsStringAsync().Result; // Replace "Cart" with your cart model
                return View(cart);
            }
            else
            {
                // Handle failure (e.g., show error message)
                TempData["ErrorMessage"] = "Failed to load cart!";
                return RedirectToAction("Index", "Home"); // Redirect to a different action
            }
        }


        //// Action to add a product to the cart
        //[HttpPost]
        //public async Task<IActionResult> AddToCart(int productId)
        //{
        //    var response = await _client.PostAsync($"{_client.BaseAddress}/{productId}", null);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        // Handle success (e.g., show success message)
        //        TempData["Message"] = "Product added to cart!";
        //    }
        //    else
        //    {
        //        // Handle failure (e.g., show error message)
        //        TempData["ErrorMessage"] = "Failed to add product to cart!";
        //    }
        //    return RedirectToAction("Index", "Home"); // Redirect to a different action
        //}


        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int cartId, int quantity)
        {
            try
            {
                // Create a CartItem object representing an item to be added to the cart
                var cartItem = new
                {
                    CartId = cartId,
                    ProductId = productId,
                    Quantity = quantity
                };

                // Serialize the CartItem object to JSON
                var jsonCartItem = JsonConvert.SerializeObject(cartItem);

                // Create StringContent from JSON
                var content = new StringContent(jsonCartItem, Encoding.UTF8, "application/json");

                // Send the request to the API endpoint for adding a cart item to the cart
                var response = await _client.PostAsync("https://localhost:7027/cart", content);

                if (response.IsSuccessStatusCode)
                {
                    // Handle success (e.g., show success message)
                    TempData["Message"] = "Product added to cart!";
                }
                else
                {
                    // Handle failure (e.g., show error message)
                    TempData["ErrorMessage"] = "Failed to add product to cart!";
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                TempData["ErrorMessage"] = "An error occurred while adding the product to the cart.";
            }

            return RedirectToAction("Index", "Home"); // Redirect to a different action
        }




        // Action to remove a product from the cart
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            var response = await _client.DeleteAsync($"api/cart/removefromcart/{productId}");
            if (response.IsSuccessStatusCode)
            {
                // Handle success (e.g., show success message)
                TempData["Message"] = "Product removed from cart!";
            }
            else
            {
                // Handle failure (e.g., show error message)
                TempData["ErrorMessage"] = "Failed to remove product from cart!";
            }
            return RedirectToAction("ViewCart");
        }
    }
}
