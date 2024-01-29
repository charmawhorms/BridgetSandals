using BridgetSandalsAPI.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BridgetSandalsAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public AnalyticsController(ApplicationDbContext db)
        {
            _db = db;
        }


        //Method to get the count of each record in the models for analytical purposes
        [HttpGet]
        public IActionResult GetCount()
        {
            int products = _db.Products.Count();
            int categories = _db.Categories.Count();
            int orders = _db.Orders.Count();

            return Ok(new { status = "success", message = "Analytics Data", totalProducts = products, totalCategories = categories, totalOrders = orders });
        }
    }
}
