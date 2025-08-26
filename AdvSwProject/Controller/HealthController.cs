using Microsoft.AspNetCore.Mvc;

namespace AdvSwProject.Controllers
{
    public class HealthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult badael() { return View(); }
        public IActionResult hasasea() { return View(); }
        public IActionResult page2() { return View(); }
        public IActionResult wasafat() { return View(); }
    }
}
