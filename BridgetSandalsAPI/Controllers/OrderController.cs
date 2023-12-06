using BridgetSandalsAPI.Data;
using BridgetSandalsAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BridgetSandalsAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public OrderController(ApplicationDbContext db)
        {
            _db = db;
        }


        //Method to get all the orders
        [HttpGet]
        public IActionResult GetOrders()
        {
            var orders = _db.Orders.Include(x => x.Customer).Include(x => x.Parish).ToList();
            if (orders == null)
            {
                return NotFound();
            }

            return Ok(orders);
        }


        //Method to get an order by id
        [HttpGet("{id}")]
        public IActionResult GetOrderById(int id)
        {
            var order = _db.Orders.FirstOrDefault(x => x.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }


        //Method to add an order to the database
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _db.Orders.AddAsync(order);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest("Failed to create order.");
            }

            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
        }


        //Method to update an order in the database
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] Order order)
        {
            if (id != order.Id)
            {
                return NotFound("Order not found");
            }

            _db.Orders.Update(order);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int id)
        {
            var order = _db.Orders.FirstOrDefault(x => x.Id == id);
            if (order == null)
            {
                return NotFound("Order not found");
            }

            _db.Orders.Remove(order);
            _db.SaveChanges();

            return Ok(order);
        }
    }
}
