using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCProject.Data;
using MVCProject.Models;

namespace MVCProject.Areas.Panel.Controllers
{
    [Area("panel")]
    [Authorize(Roles = "Admin , SuperAdmin")]
    public class CategoryController : Controller
    {
        private readonly WebAppContext _context;
        public CategoryController(WebAppContext context)
        {
            _context = context;

        }
        public IActionResult Index()
        {
            List<Category> categories = _context.Categories.ToList();
            return View(categories);
        }

        public IActionResult Create() { return View(); }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(Category category)
        {
            if (category == null) return NotFound();
            if (!ModelState.IsValid) return View();

            _context.Categories.Add(category);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null) return NotFound();
            return View(category);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(int? id,Category category)
        {
            if (!ModelState.IsValid) return View();
            if(id == null || id == 0)return NotFound();
            Category existCategory = _context.Categories.FirstOrDefault(x => x.Id == id);
            if (existCategory == null) return NotFound();
            existCategory.CategoryName = category.CategoryName;
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();
            var category = _context.Categories.FirstOrDefault(c=>c.Id== id);
            if (category == null) return NotFound();
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
