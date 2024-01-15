using Microsoft.AspNetCore.Mvc;

namespace BridgetSandalsClient.Controllers
{
    public class GiftCardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
