using Microsoft.AspNetCore.Mvc;

namespace BridgetSandalsClient.Controllers
{
    //[Authorize] - the user should be a customer
    //meaning they have to be signed in before they can 
    //order a product
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
