using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            List<Course> courses = _context.Courses.Take(3).ToList();
            List<Blog> blogs= _context.Blogs.Take(3).ToList();
            List<Event> events = _context.Events
                .Include(e=>e.EventSpeakers)
                .ThenInclude(es=>es.Speaker)
                .ToList();  
            HomeVM homeVM = new HomeVM();
            homeVM.Sliders = sliders;
            homeVM.Notices = notices;
            homeVM.NoticesInfo = noticeInfos;
            homeVM.Courses= courses;
            homeVM.Events= events;
            homeVM.Blogs= blogs;

            return View(homeVM);
        }

       
    }
}