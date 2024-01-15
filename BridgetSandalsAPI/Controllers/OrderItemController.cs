using BridgetSandalsAPI.Data;
using BridgetSandalsAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BridgetSandalsAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public OrderItemController(ApplicationDbContext db)
        {
            _db = db;
        }


        //Method to get all the order items
        [HttpGet]
        public IActionResult GetOrderItems()
        {
            var orderItems = _db.OrderItems.ToList();
            if (orderItems == null)
            {
                return NotFound();
            }

            return Ok(orderItems);
        }


        //Method to get an order item by id
        [HttpGet("{id}")]
        public IActionResult GetOrderItemById(int id)
        {
            var orderItem = _db.OrderItems.FirstOrDefault(oi => oi.Id == id);
            if (orderItem == null)
            {
                return NotFound();
            }

            return Ok(orderItem);
        }


        //Method to add an order item to the database
        [HttpPost]
        public async Task<IActionResult> CreateOrderItem([FromBody] OrderItem orderItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _db.OrderItems.AddAsync(orderItem);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest("Failed to create order item.");
            }

            return CreatedAtAction(nameof(GetOrderItemById), new { id = orderItem.Id }, orderItem);
        }


        //Method to update an order item in the database
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderItem(int id, [FromBody] OrderItem orderItem)
        {
            if (id != orderItem.Id)
            {
                return NotFound("Order item not found");
            }

            _db.OrderItems.Update(orderItem);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrderItemById), new { id = orderItem.Id }, orderItem);
        }


        //Method to delete an order item from the database by id
        [HttpDelete("{id}")]
        public IActionResult DeleteOrderItem(int id)
        {
            var orderItem = _db.OrderItems.FirstOrDefault(oi => oi.Id == id);
            if (orderItem == null)
            {
                return NotFound("Order item not found");
            }

            _db.OrderItems.Remove(orderItem);
            _db.SaveChanges();

            return Ok(orderItem);
        }
    }
}
