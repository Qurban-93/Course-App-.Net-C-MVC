using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MVCProject.Data;
using MVCProject.Models;
using MVCProject.ViewModels;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

namespace MVCProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly WebAppContext _context;
        private readonly UserManager<AppUser> _userManager;

        public HomeController(WebAppContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            List<Slider> sliders = _context.Sliders.ToList();
            List<Notice> notices = _context.Notices.ToList();
            List<NoticeInfo> noticeInfos = _context.NoticeInfos.ToList();
            List<Course> courses = _context.Courses.Take(3).ToList();
            List<Blog> blogs = _context.Blogs.Take(3).ToList();
            List<Event> events = _context.Events
                .Include(e => e.EventSpeakers)
                .ThenInclude(es => es.Speaker)
                .ToList();
            HomeVM homeVM = new HomeVM();
            homeVM.Sliders = sliders;
            homeVM.Notices = notices;
            homeVM.NoticesInfo = noticeInfos;
            homeVM.Courses = courses;
            homeVM.Events = events;
            homeVM.Blogs = blogs;

            return View(homeVM);
        }

        public async Task<IActionResult> SubscribeAdd(string? email)
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");
            if (email.IsNullOrEmpty())
            {
                TempData["Error"] = "Please enter your Email !";
                return RedirectToAction(nameof(Index));
            }
            AppUser? user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                TempData["Error"] = " Your profile is not found ! Please write to the support service";
                return RedirectToAction(nameof(Index));
            }
            if (user.Email != email)
            {
                TempData["Error"] = "This Email is not your !";
                return RedirectToAction(nameof(Index));
            }

            user.Subscribe = true;

            await _context.SaveChangesAsync();
            TempData["Success"] = "Your successfully subscribe !";

            return RedirectToAction(nameof(Index));
        }

    }
}