using BridgetSandalsAPI.Data;
using BridgetSandalsAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BridgetSandalsAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public CartController(ApplicationDbContext db)
        {
            _db = db;
        }



        //Method to get all the carts
        [HttpGet]
        public IActionResult GetCarts()
        {
            var carts = _db.Carts.Include(c => c.CartItems).ToList();
            if (carts == null)
            {
                return NotFound();
            }

            return Ok(carts);
        }


        //Method to get a cart by id
        [HttpGet("{id}")]
        public IActionResult GetCartById(int id)
        {
            var cart = _db.Carts.Include(c => c.CartItems).FirstOrDefault(c => c.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return Ok(cart);
        }


        //Method to add a cart to the database
        [HttpPost]
        public async Task<IActionResult> CreateCart([FromBody] Cart cart)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _db.Carts.AddAsync(cart);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest("Failed to create cart.");
            }

            return CreatedAtAction(nameof(GetCartById), new { id = cart.Id }, cart);
        }


        //Method to update a cart in the database
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCart(int id, [FromBody] Cart cart)
        {
            if (id != cart.Id)
            {
                return NotFound("Cart not found");
            }

            _db.Carts.Update(cart);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCartById), new { id = cart.Id }, cart);
        }


        //Method to delete a cart from the database by id
        [HttpDelete("{id}")]
        public IActionResult DeleteCart(int id)
        {
            var cart = _db.Carts.FirstOrDefault(c => c.Id == id);
            if (cart == null)
            {
                return NotFound("Cart not found");
            }

            _db.Carts.Remove(cart);
            _db.SaveChanges();

            return Ok(cart);
        }
    }
}
