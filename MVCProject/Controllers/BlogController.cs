using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCProject.Data;
using MVCProject.Models;

namespace MVCProject.Controllers
{
    public class BlogController : Controller
    {
        private readonly WebAppContext _context;
        public BlogController(WebAppContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Blog> blogs = await _context.Blogs.ToListAsync();
            return View(blogs);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id == 0) return NotFound(); 
            Blog? blog = await _context.Blogs.Include(b=>b.Comments)
                .FirstOrDefaultAsync(b => b.Id == id);
            if (blog == null) return NotFound();
            ViewBag.Blogs = ViewBag.Blogs = _context.Blogs.Take(4).ToList();


            return View(blog);
        }
    }
}
