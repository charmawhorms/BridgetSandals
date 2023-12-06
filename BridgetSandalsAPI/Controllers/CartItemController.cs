using BridgetSandalsAPI.Data;
using BridgetSandalsAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BridgetSandalsAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public CartItemController(ApplicationDbContext db)
        {
            _db = db;
        }


        //Method to get all the cart items
        [HttpGet]
        public IActionResult GetCartItems()
        {
            var cartItems = _db.CartItems.ToList();
            if (cartItems == null)
            {
                return NotFound();
            }

            return Ok(cartItems);
        }


        //Method to get a cart item by id
        [HttpGet("{id}")]
        public IActionResult GetCartItemById(int id)
        {
            var cartItem = _db.CartItems.FirstOrDefault(ci => ci.Id == id);
            if (cartItem == null)
            {
                return NotFound();
            }

            return Ok(cartItem);
        }


        //Method to add a cart item to the database
        [HttpPost]
        public async Task<IActionResult> CreateCartItem([FromBody] CartItem cartItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _db.CartItems.AddAsync(cartItem);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest("Failed to create cart item.");
            }

            return CreatedAtAction(nameof(GetCartItemById), new { id = cartItem.Id }, cartItem);
        }


        //Method to update a cart item in the database
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCartItem(int id, [FromBody] CartItem cartItem)
        {
            if (id != cartItem.Id)
            {
                return NotFound("Cart item not found");
            }

            _db.CartItems.Update(cartItem);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCartItemById), new { id = cartItem.Id }, cartItem);
        }


        //Method to delete a cart item from the database by id
        [HttpDelete("{id}")]
        public IActionResult DeleteCartItem(int id)
        {
            var cartItem = _db.CartItems.FirstOrDefault(ci => ci.Id == id);
            if (cartItem == null)
            {
                return NotFound("Cart item not found");
            }

            _db.CartItems.Remove(cartItem);
            _db.SaveChanges();

            return Ok(cartItem);
        }
    }
}
