using BridgetSandalsClient.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace BridgetSandalsClient.Controllers
{
    public class CategoryController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7027/category");
        Uri baseAddress2 = new Uri("https://localhost:7109/Auth");
        private readonly HttpClient _client;
        private readonly HttpClient _client2;

        public CategoryController()
        {
            _client = new HttpClient();
            _client2 = new HttpClient();
            _client.BaseAddress = baseAddress;
            _client2.BaseAddress = baseAddress2;
        }

        //Method for token / session
        [HttpGet]
        private string RetrieveTokenFromSession()
        {
            string token = HttpContext.Session.GetString("SessionAuth")!;
            return token;
        }

        [HttpGet]
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

            var category = response.Content.ReadAsStringAsync().Result;
            var categoryList = JsonConvert.DeserializeObject<List<Category>>(category);

            return View(categoryList);
        }


        [HttpGet]
        public IActionResult Create()
        {
            string token = RetrieveTokenFromSession();

            if (string.IsNullOrEmpty(token))
            {
                //HttpContext.Session.SetString("returnUrl", returnUrl ?? Url.Content("~/"));
                return RedirectToAction("Index", "Login");
            }
            _client2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return View();
        }


        [HttpPost]
        public IActionResult Create(Category category)
        {
            string token = RetrieveTokenFromSession();

            if (string.IsNullOrEmpty(token))
            {
                //HttpContext.Session.SetString("returnUrl", returnUrl ?? Url.Content("~/"));
                return RedirectToAction("Index", "Login");
            }
            _client2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            if (!ModelState.IsValid) return View(category);

            var json = JsonConvert.SerializeObject(category);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = _client.PostAsync(_client.BaseAddress, data).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Category creation failed");
                return View(category);
            }
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            string token = RetrieveTokenFromSession();

            if (string.IsNullOrEmpty(token))
            {
                //HttpContext.Session.SetString("returnUrl", returnUrl ?? Url.Content("~/"));
                return RedirectToAction("Index", "Login");
            }
            _client2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = _client.GetAsync($"{_client.BaseAddress}/{id}").Result;
            var response2 = response.Content.ReadAsStringAsync().Result;
            var category = JsonConvert.DeserializeObject<Category>(response2);

            return View(category);
        }



        [HttpPost]
        public IActionResult Edit(Category category)
        {
            string token = RetrieveTokenFromSession();

            if (string.IsNullOrEmpty(token))
            {
                //HttpContext.Session.SetString("returnUrl", returnUrl ?? Url.Content("~/"));
                return RedirectToAction("Index", "Login");
            }
            _client2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonConvert.SerializeObject(category);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = _client.PutAsync($"{_client.BaseAddress}/{category.Id}", data).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Category update failed");
                return View(category);
            }
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

            Category category = new Category();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/" + id.ToString()).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                category = JsonConvert.DeserializeObject<Category>(data);
            }
            return View(category);
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
