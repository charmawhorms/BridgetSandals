using BridgetSandalsClient.Models;
using BridgetSandalsClient.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace BridgetSandalsClient.Controllers
{
    public class AdminProductController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7027/product");
        Uri baseAddress2 = new Uri("https://localhost:7109/Auth");
        private readonly HttpClient _client;
        private readonly HttpClient _client2;

        public AdminProductController()
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
            var product = response.Content.ReadAsStringAsync().Result;
            var productList = JsonConvert.DeserializeObject<List<Product>>(product);

            var response2 = _client.GetAsync("https://localhost:7027/category").Result;
            var category = response2.Content.ReadAsStringAsync().Result;
            var categoryList = JsonConvert.DeserializeObject<List<Category>>(category);

            var productCategoryVM = new ProductCategoryVM
            {
                Products = productList,
                Categories = categoryList
            };

            return View(productCategoryVM);
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

            var categoryResponse = _client.GetAsync($"https://localhost:7027/category").Result;
            var categories = categoryResponse.Content.ReadAsStringAsync().Result;

            List<Category> catList = JsonConvert.DeserializeObject<List<Category>>(categories);

            var viewModel = new ProductVM
            {
                CategoryList = catList.Select(category => new SelectListItem
                {
                    Text = category.Name,
                    Value = category.Id.ToString()
                }).ToList()
            };

            return View(viewModel);
        }


        //Function to return view model
        public ProductVM fetchDefault()
        {
            var categoryResponse = _client.GetAsync($"https://localhost:7027/category").Result;
            var categories = categoryResponse.Content.ReadAsStringAsync().Result;

            List<Category> catList = JsonConvert.DeserializeObject<List<Category>>(categories);

            var viewModel = new ProductVM
            {
                CategoryList = catList.Select(category => new SelectListItem
                {
                    Text = category.Name,
                    Value = category.Id.ToString()
                }).ToList()
            };

            return viewModel;
        }


        [HttpPost]
        public async Task<IActionResult> Create(ProductVM vm)
        {
            string token = RetrieveTokenFromSession();

            if (string.IsNullOrEmpty(token))
            {
                //HttpContext.Session.SetString("returnUrl", returnUrl ?? Url.Content("~/"));
                return RedirectToAction("Index", "Login");
            }
            _client2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            if (!ModelState.IsValid)
            {
                var pVM = fetchDefault();
                return View(pVM);
            }

            var product = new ProductCreateDTO
            {
                Name = vm.Name,
                ShortDescription = vm.ShortDescription,
                Description = vm.Description,
                Price = vm.Price,
                Quantity = vm.Quantity,
                CategoryId = vm.SelectedCategoryId,
                ProductImageFile = vm.ProductImageFile
            };

            //Call the method to send the DTO with the info to the API
            var response = await SendProductToApi(product);

            //var json = JsonConvert.SerializeObject(student);
            //var data = new StringContent(json, Encoding.UTF8, "application/json");
            //var response = _client.PostAsync(_client.BaseAddress, data).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Product creation failed");
                return View(vm);
            }
        }


        private async Task<HttpResponseMessage> SendProductToApi(ProductCreateDTO productDto)
        {
            using (var formData = new MultipartFormDataContent())
            {
                // Set the Content-Type header to "multipart/form-data"
                formData.Headers.ContentType.MediaType = "multipart/form-data";

                //Add student data to the request
                formData.Add(new StringContent(productDto.Name), "Name");
                formData.Add(new StringContent(productDto.ShortDescription), "ShortDescription");
                formData.Add(new StringContent(productDto.Description), "Description");
                formData.Add(new StringContent(productDto.Price.ToString()), "Price");
                formData.Add(new StringContent(productDto.Quantity.ToString()), "Quantity");
                formData.Add(new StringContent(productDto.CategoryId.ToString()), "CategoryId");

                // Add the file to the request
                if (productDto.ProductImageFile != null && productDto.ProductImageFile.Length > 0)
                {
                    formData.Add(new StreamContent(productDto.ProductImageFile.OpenReadStream())
                    {
                        Headers = { ContentLength = productDto.ProductImageFile.Length,
                                ContentType = new MediaTypeHeaderValue(
                                    productDto.ProductImageFile.ContentType)
                            }
                    }, "ProductImageFile", productDto.ProductImageFile.FileName);
                }

                //Send to API code
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));

                //Send the data to the API
                return await _client.PostAsync(_client.BaseAddress, formData);
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

            var product = JsonConvert.DeserializeObject<Product>(response2);

            var viewModel = new ProductVM
            {
                Name = product.Name,
                ShortDescription = product.ShortDescription,
                Description = product.Description,
                Price = product.Price,
                Quantity = product.Quantity,
                SelectedCategoryId = product.CategoryId
            };

            // Populate the view model with the categories from the API
            var categoryResponse = _client.GetAsync($"https://localhost:7027/category").Result;

            var categories = categoryResponse.Content.ReadAsStringAsync().Result;

            List<Category> catList = JsonConvert.DeserializeObject<List<Category>>(categories);

            viewModel.CategoryList = catList.Select(category => new SelectListItem
            {
                Text = category.Name,
                Value = category.Id.ToString()
            }).ToList();

            // Return the view model to the view.
            return View(viewModel);
        }



        [HttpPost]
        public async Task<IActionResult> Edit(ProductVM vm)
        {
            string token = RetrieveTokenFromSession();

            if (string.IsNullOrEmpty(token))
            {
                //HttpContext.Session.SetString("returnUrl", returnUrl ?? Url.Content("~/"));
                return RedirectToAction("Index", "Login");
            }
            _client2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            if (!ModelState.IsValid)
            {
                var pVM = fetchDefault();
                return View(pVM);
            }

            var product = new ProductCreateDTO
            {
                Name = vm.Name,
                ShortDescription = vm.ShortDescription,
                Description = vm.Description,
                Price = vm.Price,
                Quantity = vm.Quantity,
                CategoryId = vm.SelectedCategoryId,
                ProductImageFile = vm.ProductImageFile
            };

            //Call the method to send the DTO with the info to the API
            var response = await SendProductToApi(product);

            //var json = JsonConvert.SerializeObject(student);
            //var data = new StringContent(json, Encoding.UTF8, "application/json");
            //var response = _client.PutAsync($"{_client.BaseAddress}/{student.Id}", data).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Product update failed");
                return View(vm);
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

            Product product = new Product();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/" + id.ToString()).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                product = JsonConvert.DeserializeObject<Product>(data);
            }

            return View(product);
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
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Handle error
                string errorMessage = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, errorMessage);
            }
        }
    }
}
