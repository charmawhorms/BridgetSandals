using BridgetSandalsAPI.Data;
using BridgetSandalsAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BridgetSandalsAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public ProductController(ApplicationDbContext db)
        {
            _db = db;
        }


        //Method to get all the products
        [HttpGet]
        public IActionResult GetProducts()
        {
            var products = _db.Products
                .Include(x => x.Category)
                .Include(x => x.Variants) // Include ProductVariants
                .ToList();

            if (products == null)
            {
                return NotFound();
            }

            //Construct the full image URL for each product
            var baseUrl = "https://localhost:7027/images/";

            //Generate the full file path for each product's image
            foreach (var product in products)
            {
                product.ProductImageFilePath = baseUrl + product.ProductImageFilePath;
            }

            return Ok(products); // Return products with variants and discounts
        }


        //Method to get a product by id
        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            var product = _db.Products.FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            var baseUrl = "https://localhost:7027/images/";

            //Generate the full file path for each product's image
            product.ProductImageFilePath = baseUrl + product.ProductImageFilePath;
            return Ok(product);
        }



        //Method to add a product to the db
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] ProductCreateDTO model)
        {
            if (ModelState.IsValid)
            {
                //Retrieve the file from the DTO
                var productImageFile = model.ProductImageFile;

                if (productImageFile != null && productImageFile.Length > 0)
                {
                    //Generate a unique file name
                    var uniqueFileName = Guid.NewGuid() + "_" + productImageFile.FileName;

                    //Define the final file path on the API server
                    var apiFilePath = Path.Combine("api", "server", "uploads", uniqueFileName);

                    //Save the file to the server
                    using (var stream = new FileStream(apiFilePath, FileMode.Create))
                    {
                        await productImageFile.CopyToAsync(stream);
                    }

                    //Store the file path to the db along with the other
                    //product details
                    var product = new Product
                    {
                        Name = model.Name,
                        ShortDescription = model.ShortDescription,
                        Description = model.Description,
                        Price = model.Price,
                        Quantity = model.Quantity,
                        CategoryId = model.CategoryId,
                        ProductImageFilePath = apiFilePath != String.Empty ? apiFilePath : ""
                            
                    };

                    // Save the product to the db using the data access logic
                    _db.Products.Add(product);

                    // Create a list to hold variants to be added to the database
                    //var variantsToAdd = new List<ProductVariant>();


                    // Add ProductVariants (if available) to the Product
                    if (model.Variants != null && model.Variants.Any())
                    {
                        foreach (var variantDto in model.Variants)
                        {
                            var variant = new ProductVariant
                            {
                                Color = variantDto.Color,
                                Size = variantDto.Size,
                                Price = variantDto.Price,
                                Quantity = variantDto.Quantity,
                                Product = product                             
                            };

                            // Add the ProductVariant entity to the context
                            _db.ProductVariants.Add(variant);
                        }
                    }

                    await _db.SaveChangesAsync();

                    return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
                }
            }
            //If ModelState is not valid or the file is missing, return a validation error response
            return BadRequest(ModelState);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductCreateDTO model)
        {
            if (ModelState.IsValid)
            {
                var existingProduct = await _db.Products.FindAsync(id);
                if (existingProduct == null)
                {
                    return NotFound(); // Or return appropriate error response for a non-existent product
                }


                // Check if a new image is uploaded
                if (model.ProductImageFile != null && model.ProductImageFile.Length > 0)
                {
                    // Process file and update image path
                    var uniqueFileName = Guid.NewGuid() + "_" + model.ProductImageFile.FileName;
                    var apiFilePath = Path.Combine("api", "server", "uploads", uniqueFileName);

                    using (var stream = new FileStream(apiFilePath, FileMode.Create))
                    {
                        await model.ProductImageFile.CopyToAsync(stream);
                    }

                    // Update product details with the new image path
                    existingProduct.ProductImageFilePath = apiFilePath;
                }

                // Update other product details
                existingProduct.Name = model.Name;
                existingProduct.ShortDescription = model.ShortDescription;
                existingProduct.Description = model.Description;
                existingProduct.Price = model.Price;
                existingProduct.Quantity = model.Quantity;
                existingProduct.CategoryId = model.CategoryId;


                // Update the existing product in the db
                _db.Products.Update(existingProduct);


                // Update product variants (if available) to the Product
                if (model.Variants != null && model.Variants.Any())
                {
                    // Remove existing variants not present in the update
                    var existingVariants = _db.ProductVariants.Where(v => v.ProductId == id).ToList();
                    foreach (var existingVariant in existingVariants)
                    {
                        var variantDto = model.Variants.FirstOrDefault(v => v.Id == existingVariant.Id);
                        if (variantDto == null)
                        {
                            _db.ProductVariants.Remove(existingVariant);
                        }
                    }

                    // Update or Add new variants
                    foreach (var variantDto in model.Variants)
                    {
                        if (variantDto.Id > 0)
                        {
                            var existingVariant = await _db.ProductVariants.FindAsync(variantDto.Id);
                            if (existingVariant != null)
                            {
                                existingVariant.Color = variantDto.Color;
                                existingVariant.Size = variantDto.Size;
                                existingVariant.Price = variantDto.Price;
                                existingVariant.Quantity = variantDto.Quantity;
                            }
                            else
                            {
                                // Create new variant
                                var newVariant = new ProductVariant
                                {
                                    ProductId = id,
                                    Color = variantDto.Color,
                                    Size = variantDto.Size,
                                    Price = variantDto.Price,
                                    Quantity = variantDto.Quantity
                                };
                                _db.ProductVariants.Add(newVariant);
                            }
                        }
                        else
                        {
                            // Create new variant
                            var newVariant = new ProductVariant
                            {
                                ProductId = id,
                                Color = variantDto.Color,
                                Size = variantDto.Size,
                                Price = variantDto.Price,
                                Quantity = variantDto.Quantity
                            };

                            _db.ProductVariants.Add(newVariant);
                        }
                    }
                }


                await _db.SaveChangesAsync();

                return Ok(existingProduct); // Return the updated product
            }
            //If ModelState is not valid or the file is missing, return a validation error response
            return BadRequest(ModelState);
        }



        //Method to delete a product by id
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = _db.Products.FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            _db.Products.Remove(product);
            _db.SaveChanges();

            return Ok(product);
        }
    }
}
