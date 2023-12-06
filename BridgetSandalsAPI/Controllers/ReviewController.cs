using BridgetSandalsAPI.Data;
using BridgetSandalsAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BridgetSandalsAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public ReviewController(ApplicationDbContext db)
        {
            _db = db;
        }


        //Method to get all the reviews
        [HttpGet]
        public IActionResult GetReviews()
        {
            var reviews = _db.Reviews.Include(x => x.Customer).Include(x => x.Product).ToList();
            if (reviews == null)
            {
                return NotFound();
            }

            return Ok(reviews);
        }


        //Method to get a review by id
        [HttpGet("{id}")]
        public IActionResult GetReviewById(int id)
        {
            var review = _db.Reviews.FirstOrDefault(x => x.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            return Ok(review);
        }


        //Method to add a review to the database
        [HttpPost]
        public async Task<IActionResult> CreateReview([FromBody] Review review)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _db.Reviews.AddAsync(review);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Return a bad request response
                return BadRequest("Failed to create review.");
            }

            return CreatedAtAction(nameof(GetReviewById), new { id = review.Id }, review);
        }



        //Method that updates a review based on the id in the database
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReview(int id, [FromBody] Review review)
        {
            if (id != review.Id)
            {
                return NotFound("Review not found");
            }

            _db.Reviews.Update(review);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReviewById), new { id = review.Id }, review);
        }


        //Method that deletes a review made based on the id from the database
        [HttpDelete("{id}")]
        public IActionResult DeleteReview(int id)
        {
            var review = _db.Reviews.FirstOrDefault(x => x.Id == id);
            if (review == null)
            {
                return NotFound("Review not found");
            }
            _db.Reviews.Remove(review);
            _db.SaveChanges();

            return Ok(review);
        }
    }
}
