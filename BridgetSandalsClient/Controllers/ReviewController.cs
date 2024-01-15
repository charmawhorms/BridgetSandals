using Microsoft.AspNetCore.Mvc;

namespace BridgetSandalsClient.Controllers
{
    public class ReviewController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
