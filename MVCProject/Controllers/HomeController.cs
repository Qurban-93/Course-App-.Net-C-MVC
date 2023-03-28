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
            HomeVM homeVM = new HomeVM();
            homeVM.Sliders = sliders;
            return View(homeVM);
        }

       
    }
}