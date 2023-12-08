using BridgetSandalsClient.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace BridgetSandalsClient.Controllers
{
    public class HomeController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7027/product");
        private readonly HttpClient _client;

        private readonly ILogger<HomeController> _logger;


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }


        [HttpGet]
        public IActionResult Index()
        {
            var response = _client.GetAsync(_client.BaseAddress).Result;

            var product = response.Content.ReadAsStringAsync().Result;
            var productList = JsonConvert.DeserializeObject<List<Product>>(product);

            return View(productList);
        }





        public IActionResult Privacy()
        {
            return View();
        }


        public IActionResult AboutUs()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}