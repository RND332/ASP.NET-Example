using Microsoft.AspNetCore.Mvc;

namespace TockenomicsMVC.Controllers
{
    public class EconomyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
