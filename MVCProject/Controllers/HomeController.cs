using Microsoft.AspNetCore.Mvc;
using MVCProject.Data;
using MVCProject.Models;
using MVCProject.ViewModels;
using System.Diagnostics;

namespace MVCProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly WebAppContext _context;
        public HomeController(WebAppContext context)
        {
            _context= context;
        }
      
        public IActionResult Index()
        {
            List<Slider> sliders = _context.Sliders.ToList();
            List<Notice> notices= _context.Notices.ToList();
            List<NoticeInfo> noticeInfos= _context.NoticeInfos.ToList();
            List<Course> courses = _context.Courses.ToList();
            HomeVM homeVM = new HomeVM();
            homeVM.Sliders = sliders;
            homeVM.Notices = notices;
            homeVM.NoticesInfo = noticeInfos;
            homeVM.Courses= courses;
            return View(homeVM);
        }

       
    }
}