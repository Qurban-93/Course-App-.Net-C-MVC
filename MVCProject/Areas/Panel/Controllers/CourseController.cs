using Microsoft.AspNetCore.Mvc;
using MVCProject.Data;
using MVCProject.Extencions;
using MVCProject.Models;
using MVCProject.ViewModels.CourseVMs;
using MVCProject.ViewModels.SliderVMs;

namespace MVCProject.Areas.Panel.Controllers
{
    [Area("Panel")]
    public class CourseController : Controller
    {
        private readonly WebAppContext _appContext;
        private readonly IWebHostEnvironment _env;

        public CourseController(WebAppContext appContext, IWebHostEnvironment env)
        {
            _appContext = appContext;
            _env = env;
        }

        public IActionResult Index()
        {
            return View(_appContext.Courses.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(CourseCreateVM courseCreateVM)
        {
            if (courseCreateVM == null) return NotFound();
            if (!ModelState.IsValid) return View();
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

            Course newCourse = new Course();
            newCourse.ImageUrl = courseCreateVM.Photo.SaveImage(_env,"img","course");
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
            newCourse.Language= courseCreateVM.Language;
            ViewBag.Succesful = "Course Added !";
            
            _appContext.Courses.Add(newCourse);
            _appContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
