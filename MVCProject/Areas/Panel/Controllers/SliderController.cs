using Microsoft.AspNetCore.Mvc;
using MVCProject.Data;

namespace MVCProject.Areas.Panel.Controllers
{
    [Area("Panel")]
    public class SliderController : Controller
    {
        private readonly WebAppContext _appContext;

        public SliderController(WebAppContext appContext) 
        {
            _appContext= appContext;
        }

        public IActionResult Index()
        {
            return View(_appContext.Sliders.ToList());
        }
    }
}
