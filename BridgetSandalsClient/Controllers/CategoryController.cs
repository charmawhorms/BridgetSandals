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
        private readonly HttpClient _client;

        public CategoryController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }


        [HttpGet]
        public IActionResult Index()
        {
            var response = _client.GetAsync(_client.BaseAddress).Result;

            var category = response.Content.ReadAsStringAsync().Result;
            var categoryList = JsonConvert.DeserializeObject<List<Category>>(category);

            return View(categoryList);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(Category category)
        {
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
            var response = _client.GetAsync($"{_client.BaseAddress}/{id}").Result;
            var response2 = response.Content.ReadAsStringAsync().Result;
            var category = JsonConvert.DeserializeObject<Category>(response2);

            return View(category);
        }



        [HttpPost]
        public IActionResult Edit(Category category)
        {
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
