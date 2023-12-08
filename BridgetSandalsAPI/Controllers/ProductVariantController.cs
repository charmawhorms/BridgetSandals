//using BridgetSandalsAPI.Data;
//using BridgetSandalsAPI.Models;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace BridgetSandalsAPI.Controllers
//{
//    [Route("[controller]")]
//    [ApiController]
//    public class ProductVariantController : ControllerBase
//    {
//        private readonly ApplicationDbContext _db;

//        public ProductVariantController(ApplicationDbContext db)
//        {
//            _db = db;
//        }


//        //Method to get all the product variants
//        [HttpGet]
//        public IActionResult GetProductVariants()
//        {
//            var productVariants = _db.ProductVariants.Include(x => x.Product).ToList();
//            if (productVariants == null)
//            {
//                return NotFound();
//            }

//            return Ok(productVariants);
//        }


//        //Method to get a product variants by id
//        [HttpGet("{id}")]
//        public IActionResult GetProductVariantById(int id)
//        {
//            var productVariant = _db.ProductVariants.FirstOrDefault(x => x.Id == id);
//            if (productVariant == null)
//            {
//                return NotFound();
//            }

//            return Ok(productVariant);
//        }


//        //Method to add a product variant to the database
//        [HttpPost]
//        public async Task<IActionResult> CreateProductVariants([FromBody] ProductVariant productVariant)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            try
//            {
//                await _db.ProductVariants.AddAsync(productVariant);
//                await _db.SaveChangesAsync();
//            }
//            catch (Exception ex)
//            {
//                // Return a bad request response
//                return BadRequest("Failed to create product variant.");
//            }

//            return CreatedAtAction(nameof(GetProductVariantById), new { id = productVariant.Id }, productVariant);
//        }



//        //Method that updates a product variant based on the id in the database
//        [HttpPut("{id}")]
//        public async Task<IActionResult> UpdateProductVariant(int id, [FromBody] ProductVariant productVariant)
//        {
//            if (id != productVariant.Id)
//            {
//                return NotFound("Product variant not found");
//            }

//            _db.ProductVariants.Update(productVariant);
//            await _db.SaveChangesAsync();

//            return CreatedAtAction(nameof(GetProductVariantById), new { id = productVariant.Id }, productVariant);
//        }


//        //Method that deletes a product variant made based on the id from the database
//        [HttpDelete("{id}")]
//        public IActionResult DeleteProductVariant(int id)
//        {
//            var productVariant = _db.ProductVariants.FirstOrDefault(x => x.Id == id);
//            if (productVariant == null)
//            {
//                return NotFound("Product variant not found");
//            }
//            _db.ProductVariants.Remove(productVariant);
//            _db.SaveChanges();

//            return Ok(productVariant);
//        }

//    }
//}
