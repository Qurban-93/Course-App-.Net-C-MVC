using Microsoft.AspNetCore.Mvc;

namespace MVCProject.Areas.Panel.Controllers
{
    [Area("Panel")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
