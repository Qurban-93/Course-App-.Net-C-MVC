using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCProject.Data;
using MVCProject.Models;

namespace MVCProject.Controllers
{
    public class TeacherController : Controller
    {
        private readonly WebAppContext _context;
        public TeacherController(WebAppContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Teachers.ToList());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id == 0) return NotFound();
            Teacher? teacher = await _context.Teachers.Include(t=>t.Skills)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (teacher == null) return NotFound();

            return View(teacher);
        }
    }
}
