using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCProject.Data;
using MVCProject.Extencions;
using MVCProject.Models;
using MVCProject.ViewModels.CourseVMs;
using System.Data;

namespace MVCProject.Areas.Panel.Controllers
{
    [Area("Panel")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class CourseController : Controller
    {
        private readonly WebAppContext _appContext;
        private readonly IWebHostEnvironment _env;

        public CourseController(WebAppContext appContext, IWebHostEnvironment env)
        {
            _appContext = appContext;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            return View( await _appContext.Courses
                .Include(c=>c.CourseCategories).ThenInclude(c=>c.Category)
                .ToListAsync());
        }

        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_appContext.Categories.ToList(),"Id","CategoryName");
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(CourseCreateVM courseCreateVM)
        {
            ViewBag.Categories = new SelectList(_appContext.Categories.ToList(), "Id", "CategoryName");
            if (courseCreateVM == null) return NotFound();
            if (!ModelState.IsValid) return View();
            if(_appContext.Courses.Any(c=>c.CourseName.Trim().ToLower() == courseCreateVM.CourseName.Trim().ToLower()))
            {
                ModelState.AddModelError("CourseName", "Eyni adli kurs movcuddur!");
                return View(courseCreateVM);
            }
            if (DateTime.Now > courseCreateVM.StartDate)
            {
                ModelState.AddModelError("StartDate", "Tarix kecmish ola bilmez !");
                return View();
            }

            if (!courseCreateVM.Photo.CheckImageType())
            {
                ModelState.AddModelError("Photo", "Ancaq Image Elave ele !");
                return View();
            }
            if (courseCreateVM.Photo.CheckImageSize(5))
            {
                ModelState.AddModelError("Photo", "Sekilin hecimi 5 Mgb artiq ola bilmez !");
                return View();
            }
            List<CourseCategory> categories = new List<CourseCategory>();
            foreach (var item in courseCreateVM.CategoryId)
            {
                CourseCategory category = new CourseCategory();
                category.CategoryId = item;
                categories.Add(category);

            }

            Course newCourse = new Course();
            newCourse.ImageUrl = courseCreateVM.Photo.SaveImage(_env, "img", "course");
            newCourse.CourseName = courseCreateVM.CourseName;
            newCourse.AboutCourse = courseCreateVM.AboutCourse;
            newCourse.Duration = courseCreateVM.Duration;
            newCourse.StartDate = courseCreateVM.StartDate;
            newCourse.CoursePrice = courseCreateVM.CoursePrice;
            newCourse.ClassDuration = courseCreateVM.ClassDuration;
            newCourse.StudentsCount = courseCreateVM.StudentsCount;
            newCourse.Certification = courseCreateVM.Certification;
            newCourse.Description = courseCreateVM.Description;
            newCourse.HowToApply = courseCreateVM.HowToApply;
            newCourse.SkillLevel = courseCreateVM.SkillLevel;
            newCourse.Language = courseCreateVM.Language;
            newCourse.CourseCategories = categories;
            TempData["Added"] = "ok";

            _appContext.Courses.Add(newCourse);
            await _appContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Categories = new SelectList(_appContext.Categories.ToList(), "Id", "CategoryName");
            if (id == null || id == 0) return NotFound();
            Course? course = await _appContext.Courses.FirstOrDefaultAsync(c => c.Id == id);
            if (course == null) return NotFound();


            CourseEditVM courseEditVM = new CourseEditVM();
            courseEditVM.CourseName = course.CourseName;
            courseEditVM.AboutCourse = course.AboutCourse;
            courseEditVM.Duration = course.Duration;
            courseEditVM.StartDate = course.StartDate;
            courseEditVM.CoursePrice = course.CoursePrice;
            courseEditVM.ClassDuration = course.ClassDuration;
            courseEditVM.StudentsCount = course.StudentsCount;
            courseEditVM.Certification = course.Certification;
            courseEditVM.Description = course.Description;
            courseEditVM.HowToApply = course.HowToApply;
            courseEditVM.SkillLevel = course.SkillLevel;
            courseEditVM.Language = course.Language;
            courseEditVM.ImageUrl = course.ImageUrl;


            return View(courseEditVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, CourseEditVM courseEditVM)
        {
            ViewBag.Categories = new SelectList(_appContext.Categories.ToList(), "Id", "CategoryName");
            if (id == null || id == 0) return NotFound();
            Course? course = await _appContext.Courses.Include(c=>c.CourseCategories)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (!ModelState.IsValid)
            {
                courseEditVM.ImageUrl = course.ImageUrl;
                return View(courseEditVM);
            }

           
            if (course == null) return NotFound();

            if(_appContext.Courses.Any(c=>c.CourseName.Trim().ToLower() == courseEditVM.CourseName.Trim().ToLower() && c.Id !=id))
            {
                ModelState.AddModelError("CourseName", "Bele adla kurs movcuddur!");
                courseEditVM.ImageUrl = course.ImageUrl;
                return View(courseEditVM);
            }

            if(courseEditVM.Photo != null)
            {
                if (!courseEditVM.Photo.CheckImageType())
                {
                    ModelState.AddModelError("Photo", "Ancaq Image Elave ele !");
                    return View();
                }
                if (courseEditVM.Photo.CheckImageSize(5))
                {
                    ModelState.AddModelError("Photo", "Sekilin hecimi 5 Mgb artiq ola bilmez !");
                    return View();
                }

                string fullPath = Path.Combine(_env.WebRootPath, "img", "course", course.ImageUrl);

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                else
                {
                    ModelState.AddModelError("ImageUrl", "Kohne sekil tapilmadi yeniden cehd edin !");
                    courseEditVM.ImageUrl = course.ImageUrl;
                    return View(courseEditVM);
                }

                course.ImageUrl = courseEditVM.Photo.SaveImage(_env,"img","course");
            }


            if(courseEditVM.CategoriesId != null) 
            { 
                List<CourseCategory> categories = new List<CourseCategory>();

                foreach (var item in courseEditVM.CategoriesId)
                {
                    CourseCategory courseCategory = new();
                    courseCategory.CategoryId = item;
                    categories.Add(courseCategory);

                }

                if(course.CourseCategories.Count> 0) { course.CourseCategories.Clear(); }
                course.CourseCategories= categories;
            
            }
           
            course.CourseName = courseEditVM.CourseName;
            course.AboutCourse = courseEditVM.AboutCourse;
            course.Duration = courseEditVM.Duration;
            course.StartDate = courseEditVM.StartDate;
            course.CoursePrice = courseEditVM.CoursePrice;
            course.ClassDuration = courseEditVM.ClassDuration;
            course.StudentsCount = courseEditVM.StudentsCount;
            course.Certification = courseEditVM.Certification;
            course.Description = courseEditVM.Description;
            course.HowToApply = courseEditVM.HowToApply;
            course.SkillLevel = courseEditVM.SkillLevel;
            course.Language = courseEditVM.Language;
            TempData["Edited"] = "ok";

           
            _appContext.SaveChangesAsync();


            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();
             _appContext.Courses.Remove( await _appContext.Courses.FirstOrDefaultAsync(c => c.Id == id));
            await _appContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id == 0) return NotFound();
            Course? course = await _appContext.Courses.FirstOrDefaultAsync(c=>c.Id == id);
            if (course == null) return NotFound();
            
            return View(course);
        }
    }
}
