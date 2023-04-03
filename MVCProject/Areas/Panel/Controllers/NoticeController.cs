using Microsoft.AspNetCore.Mvc;
using MVCProject.Data;

namespace MVCProject.Areas.Panel.Controllers
{
    [Area("Panel")]
    public class NoticeController : Controller
    {
        private readonly WebAppContext _webAppContext;

        public NoticeController(WebAppContext webAppContext)
        {
            _webAppContext = webAppContext;
        }

        public IActionResult Index()
        {
       
            return View(_webAppContext.Notices.ToList());
        }
    }
}
