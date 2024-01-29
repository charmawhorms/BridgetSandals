using BridgetSandalsClient.Models;
using BridgetSandalsClient.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;

namespace BridgetSandalsClient.Controllers
{
    public class OrderController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7027/order");
        Uri baseAddress2 = new Uri("https://localhost:7109/Auth");
        private readonly HttpClient _client;
        private readonly HttpClient _client2;


        public OrderController()
        {
            _client = new HttpClient();
            _client2 = new HttpClient();
            _client.BaseAddress = baseAddress;
            _client2.BaseAddress = baseAddress2;
        }

        [HttpGet]
        private string RetrieveTokenFromSession()
        {
            string token = HttpContext.Session.GetString("SessionAuth")!;
            return token;
        }

        public IActionResult Index()
        {
            string token = RetrieveTokenFromSession();

            if (string.IsNullOrEmpty(token))
            {
                //HttpContext.Session.SetString("returnUrl", returnUrl ?? Url.Content("~/"));
                return RedirectToAction("Index", "Login");
            }
            _client2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = _client.GetAsync(_client.BaseAddress).Result;

            var order = response.Content.ReadAsStringAsync().Result;
            var orderList = JsonConvert.DeserializeObject<List<Order>>(order);

            return View(orderList);
        }


        [HttpPost]
        public IActionResult PlaceOrder(Order order)
        {
            string token = RetrieveTokenFromSession();

            if (string.IsNullOrEmpty(token))
            {
                //HttpContext.Session.SetString("returnUrl", returnUrl ?? Url.Content("~/"));
                return RedirectToAction("Index", "Login");
            }
            _client2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            if (!ModelState.IsValid) return View(order);

            // Retrieving cart items from the session
            var orderItems = HttpContext.Session.Get<List<OrderItem>>("Cart");

            if (orderItems != null)
            {
                // Assign the order items to the order and set OrderId for each item
                order.OrderItems = orderItems.Select(item =>
                {
                    item.OrderId = order.Id; // Set OrderId for each item
                    return item;
                }).ToList();

                // Clearing the cart after placing the order
                HttpContext.Session.Remove("Cart");
            }

            // Serialize the order with OrderItems
            var json = JsonConvert.SerializeObject(order);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = _client.PostAsync(_client.BaseAddress, data).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Product");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Failed to place order");
                return View(order);
            }
        }


        [HttpGet]
        public IActionResult Details(int id)
        {
            string token = RetrieveTokenFromSession();

            if (string.IsNullOrEmpty(token))
            {
                //HttpContext.Session.SetString("returnUrl", returnUrl ?? Url.Content("~/"));
                return RedirectToAction("Index", "Login");
            }
            _client2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            Order order = new Order();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/" + id.ToString()).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                order = JsonConvert.DeserializeObject<Order>(data);
            }

            return View(order);
        }


        [HttpGet]
        public IActionResult Delete(int id)
        {
            string token = RetrieveTokenFromSession();

            if (string.IsNullOrEmpty(token))
            {
                //HttpContext.Session.SetString("returnUrl", returnUrl ?? Url.Content("~/"));
                return RedirectToAction("Index", "Login");
            }
            _client2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            Order order = new Order();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/" + id.ToString()).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                order = JsonConvert.DeserializeObject<Order>(data);
            }
            return View(order);
        }


        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string token = RetrieveTokenFromSession();

            if (string.IsNullOrEmpty(token))
            {
                //HttpContext.Session.SetString("returnUrl", returnUrl ?? Url.Content("~/"));
                return RedirectToAction("Index", "Login");
            }
            _client2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _client.DeleteAsync(_client.BaseAddress + "/" + id.ToString());

            if (response.IsSuccessStatusCode)
            {
                // Handle success
            }
            else
            {
                // Handle error
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
