using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCProject.Data;
using MVCProject.Models;
using System.Collections.Generic;

namespace MVCProject.Controllers
{
    public class BlogController : Controller
    {
        private readonly WebAppContext _context;
        public BlogController(WebAppContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int take = 3, int page = 1)
        {
            List<Blog> blogs = await _context.Blogs
                .Skip(take*(page-1))
                .Take(take)
                .Include(b=>b.Comments.OrderByDescending(c=>c.Id)).OrderByDescending(b=>b.Id)
                .ToListAsync();
            ViewBag.Pages = (int)Math.Ceiling((decimal)_context.Blogs.Count() / take);
            ViewBag.CurrentPage = page;
            return View(blogs);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id == 0) return NotFound(); 
            Blog? blog = await _context.Blogs.Include(b=>b.Comments).ThenInclude(c=>c.User)
                .FirstOrDefaultAsync(b => b.Id == id);
            if (blog == null) return NotFound();
            ViewBag.Blogs = ViewBag.Blogs = _context.Blogs.Take(4).ToList();


            return View(blog);
        }
    }
}
