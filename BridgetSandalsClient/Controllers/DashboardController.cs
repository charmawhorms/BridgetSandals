using Microsoft.AspNetCore.Mvc;

namespace BridgetSandalsClient.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
