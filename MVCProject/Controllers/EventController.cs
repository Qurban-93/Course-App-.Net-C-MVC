using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCProject.Data;
using MVCProject.Models;

namespace MVCProject.Controllers
{
    public class EventController : Controller
    {
        private readonly WebAppContext _context;
        public EventController(WebAppContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Event> events = _context.Events.ToList();
            return View(events);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id == 0)  return NotFound(); 
            Event event1 = await _context.Events
                .Include(e=>e.EventSpeakers).ThenInclude(es=>es.Speaker)
                .FirstOrDefaultAsync(e => e.Id == id);
            if (event1 == null)  return NotFound();
            ViewBag.Courses = _context.Courses.OrderByDescending(c => c.Id).Take(3).ToList();
            return View(event1);
        }
    }
}
