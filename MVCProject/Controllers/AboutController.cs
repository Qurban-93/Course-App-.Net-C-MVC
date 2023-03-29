using Microsoft.AspNetCore.Mvc;
using MVCProject.Data;
using MVCProject.ViewModels;

namespace MVCProject.Controllers
{
    public class AboutController : Controller
    {
        private readonly WebAppContext _context;
        public AboutController(WebAppContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            AboutVM aboutVM= new AboutVM();
            aboutVM.Teachers = _context.Teachers.Take(4).ToList();
            aboutVM.Notices= _context.Notices.ToList();
            return View(aboutVM);
        }
    }
}
