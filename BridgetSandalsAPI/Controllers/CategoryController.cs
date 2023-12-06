using BridgetSandalsAPI.Data;
using BridgetSandalsAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BridgetSandalsAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }


        //Method to get all the categories
        [HttpGet]
        public IActionResult GetCategories()
        {
            var categories = _db.Categories.ToList();
            if (categories == null)
            {
                return NotFound();
            }

            return Ok(categories);
        }


        //Method to get a category by id
        [HttpGet("{id}")]
        public IActionResult GetCategoryById(int id)
        {
            var category = _db.Categories.FirstOrDefault(x => x.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }


        //Method to add a category to the database
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _db.Categories.AddAsync(category);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest("Failed to create category.");
            }

            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
        }


        //Method to update a category in the database
        [HttpPut("{id}")]
        public IActionResult UpdateCategory(int id, [FromBody] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            _db.Categories.Update(category);
            _db.SaveChanges();

            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
        }


        //Method to delete a category from the database by id
        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var category = _db.Categories.FirstOrDefault(x => x.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            _db.Categories.Remove(category);
            _db.SaveChanges();

            return Ok(category);
        }
    }
}
