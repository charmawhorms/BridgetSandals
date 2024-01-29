using BridgetSandalsClient.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace BridgetSandalsClient.Controllers
{
    public class LoginController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7027/Auth");
        private readonly HttpClient _client;

        public LoginController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }


        public IActionResult Index()
        {
            User user = new User();
            return View(user);
        }


        [HttpPost]
        public async Task<IActionResult> Index(User user)
        {
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync($"{_client.BaseAddress}/login", content);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
                if (responseData.ContainsKey("status") && responseData["status"].ToString() == "success")
                {
                    //if (responseData.ContainsKey("data") && responseData["data"] is JObject jObject)
                    //{
                    //var token = jObject.GetValue("result").ToString();
                    var token = responseData["data"].ToString();
                    HttpContext.Session.SetString("SessionAuth", token);

                    var returnUrl = HttpContext.Session.GetString("returnUrl");
                    if (returnUrl == null)
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }
                    return Redirect(returnUrl);
                    //}
                }
                //TempData["errorMessage"] = "The username or password you've entered is incorrect";

                ViewBag.LoginError = "The username or password you've entered is incorrect";
            }
            return View(user);
        }


        public async Task<IActionResult> LogOut()
        {
            if (ModelState.IsValid)
            {
                HttpContext.Session.Remove("SessionAuth");
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
    }
}
