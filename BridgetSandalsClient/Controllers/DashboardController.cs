using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace BridgetSandalsClient.Controllers
{
    public class DashboardController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7027/analytics");
        Uri baseAddress2 = new Uri("https://localhost:7027/Auth");
        private readonly HttpClient _client;
        private readonly HttpClient _client2;

        public DashboardController()
        {
            _client = new HttpClient();
            _client2 = new HttpClient();
            _client.BaseAddress = baseAddress;
            _client2.BaseAddress = baseAddress2;
        }

        public IActionResult PassSessionUsername()
        {
            var username = HttpContext.Session.GetString("SessionUsername");
            ViewBag.LogInUser = username;

            return View();
        }

        //Method for token / session
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

            HttpResponseMessage response = _client.GetAsync(baseAddress).Result;
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);
                if (data.ContainsKey("status") && data["status"].ToString() == "success")
                {
                    var totalProducts = data["totalProducts"].ToString();
                    var totalCategories = data["totalCategories"].ToString();
                    var totalOrders = data["totalOrders"].ToString();

                    ViewBag.TotalProducts = totalProducts;
                    ViewBag.TotalCategories = totalCategories;
                    ViewBag.TotalOrders = totalOrders;
                }
            }

            return View();
        }
    }
}
