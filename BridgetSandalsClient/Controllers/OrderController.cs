using BridgetSandalsClient.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace BridgetSandalsClient.Controllers
{
    public class OrderController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7027/order");
        private readonly HttpClient _client;

        public OrderController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }


        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public IActionResult PlaceOrder(Order order)
        {
            if (!ModelState.IsValid) return View(order);

            var json = JsonConvert.SerializeObject(order);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = _client.PostAsync(_client.BaseAddress, data).Result;

            if (response.IsSuccessStatusCode)
            {
                //Clears the cart when the order is made
                HttpContext.Session.Remove("Cart");

                return RedirectToAction("Index", "Product");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Failed to place order");
                return View(order);
            }
            // Process the order and save it to the database
            // ...
            // You may want to clear the shopping cart after placing the order

            // Redirect to a confirmation page or any other appropriate action
            //return RedirectToAction("OrderConfirmation", new { orderId = order.Id });
        }
    }
}
