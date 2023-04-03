using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCProject.Data;
using MVCProject.Extencions;
using MVCProject.Models;
using MVCProject.ViewModels.SkillsVMs;
using MVCProject.ViewModels.SliderVMs;
using MVCProject.ViewModels.TeacherVMs;

namespace MVCProject.Areas.Panel.Controllers
{
    [Area("panel")]
    public class TeacherController : Controller
    {
        private readonly WebAppContext _webAppContext;
        private readonly IWebHostEnvironment _env;

        public TeacherController(WebAppContext webAppContext, IWebHostEnvironment env)
        {
            _webAppContext = webAppContext;
            _env = env;
        }

        public IActionResult Index()
        {
            List<Teacher> teachers = _webAppContext.Teachers.Include(t => t.Skills).ToList();
            return View(teachers);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(TeacherCreateVM teacherCreateVM)
        {
            if (!ModelState.IsValid) return View(teacherCreateVM);
            if (!teacherCreateVM.Photo.CheckImageType())
            {
                ModelState.AddModelError("Photo", "Ancaq Image Elave ele !");
                return View();
            }
            if (teacherCreateVM.Photo.CheckImageSize(5))
            {
                ModelState.AddModelError("Photo", "Sekilin hecimi 5 Mgb artiq ola bilmez !");
                return View();
            }

            string[] skills = teacherCreateVM.SkillsAndLevels.Split(",");
            if (skills == null || skills.Length == 0)
            {
                ModelState.AddModelError("SkillsAndLevels", "Duzgun qaydada daxil edin! ");
                return View();
            }
            List<Skill> skillList = new List<Skill>();

            foreach (var item in skills)
            {

                if (item.Split("=").Length != 2)
                {
                    ModelState.AddModelError("SkillsAndLevels", "Duzgun qaydada daxil edin! Vergul mutleqdi !");
                    return View();
                }

                string key = item.Split("=")[0].Trim();
                string value = item.Split("=")[1].Trim();
                if (value.Length > 3)
                {
                    ModelState.AddModelError("SkillsAndLevels", "Level 100 den artiq olmaz !");
                    return View();
                }
                foreach (var symbol in value)
                {
                    if (!char.IsDigit(symbol))
                    {
                        ModelState.AddModelError("SkillsAndLevels", "Beraberlikden sonra yalniz reqem daxil etmek olar !Max value :100");
                        return View();
                    }
                }
                if (value.Length == 3)
                {
                    if (value[0] != '1' || value[1] != '0')
                    {
                        ModelState.AddModelError("SkillsAndLevels", "Level 100 den artiq olmaz !");
                        return View();
                    }

                }

                value = string.Concat(value,"%");

                Skill skill = new Skill();
                skill.Key = key;
                skill.Value = value;
                skillList.Add(skill);

            }

            Teacher teacher = new Teacher();

            teacher.ImageUrl = teacherCreateVM.Photo.SaveImage(_env, "img", "teacher");
            teacher.FullName = teacherCreateVM.FullName;
            teacher.Faculty = teacherCreateVM.Faculty;
            teacher.Mail = teacherCreateVM.Mail;
            teacher.Degree = teacherCreateVM.Degree;
            teacher.Skype = teacherCreateVM.Skype;
            teacher.Experience = teacherCreateVM.Experience;
            teacher.Post = teacherCreateVM.Post;
            teacher.Hobbies = teacherCreateVM.Hobbies;
            teacher.PhoneNumber = teacherCreateVM.PhoneNumber;
            teacher.AboutMe = teacherCreateVM.AboutMe;
            teacher.Skills = skillList;

            _webAppContext.Teachers.Add(teacher);
            _webAppContext.SaveChanges();
            TempData["Added"] = "ok";

            return RedirectToAction("index");
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();
            Teacher? teacher = _webAppContext.Teachers.FirstOrDefault(t => t.Id == id);
            if (teacher == null) return NotFound();
            TeacherEditVM teacherEditVM = new TeacherEditVM();
            teacherEditVM.Post = teacher.Post;
            teacherEditVM.Faculty = teacher.Faculty;
            teacherEditVM.FullName = teacher.FullName;
            teacherEditVM.AboutMe = teacher.AboutMe;
            teacherEditVM.PhoneNumber = teacher.PhoneNumber;
            teacherEditVM.Degree = teacher.Degree;
            teacherEditVM.Hobbies = teacher.Hobbies;
            teacherEditVM.Experience = teacher.Experience;
            teacherEditVM.Mail = teacher.Mail;
            teacherEditVM.Skype = teacher.Skype;
            teacherEditVM.ImageUrl = teacher.ImageUrl;


            return View(teacherEditVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TeacherEditVM teacherEditVM, int? id)
        {
            if (teacherEditVM == null) return NotFound();
            if (!ModelState.IsValid)
            {
                teacherEditVM.ImageUrl = _webAppContext.Teachers.FirstOrDefault(x => x.Id == id).ImageUrl;
                return View(teacherEditVM);
            }
            if (id == 0 || id == null) return NotFound();

            Teacher teacher = await _webAppContext.Teachers.FirstOrDefaultAsync(t => t.Id == id);
            if (teacher == null) return NotFound();

            if (teacherEditVM.Photo != null)
            {
                if (!teacherEditVM.Photo.CheckImageType())
                {
                    ModelState.AddModelError("Photo", "Ancaq Image Elave ele !");
                    return View();
                }
                if (teacherEditVM.Photo.CheckImageSize(5))
                {
                    ModelState.AddModelError("Photo", "Sekilin hecimi 5 Mgb artiq ola bilmez !");
                    return View();
                }

                string fullPath = Path.Combine(_env.WebRootPath, "img", "teacher", teacher.ImageUrl);

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                else
                {
                    ModelState.AddModelError("ImageUrl", "Kohne sekil tapilmadi yeniden cehd edin !");
                    return View(teacherEditVM);
                }
                teacher.ImageUrl = teacherEditVM.Photo.SaveImage(_env, "img", "teacher");
            }
            teacher.Post = teacherEditVM.Post;
            teacher.Faculty = teacherEditVM.Faculty;
            teacher.FullName = teacherEditVM.FullName;
            teacher.AboutMe = teacherEditVM.AboutMe;
            teacher.PhoneNumber = teacherEditVM.PhoneNumber;
            teacher.Degree = teacherEditVM.Degree;
            teacher.Hobbies = teacherEditVM.Hobbies;
            teacher.Experience = teacherEditVM.Experience;
            teacher.Mail = teacherEditVM.Mail;
            teacher.Skype = teacherEditVM.Skype;

            await _webAppContext.SaveChangesAsync();
            TempData["Edited"] = "ok";

            return RedirectToAction("index");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();
            Teacher teacher = await _webAppContext.Teachers.FirstOrDefaultAsync(x => x.Id == id);
            if (teacher == null) return NotFound();
            _webAppContext.Teachers.Remove(teacher);
            await _webAppContext.SaveChangesAsync();

            return RedirectToAction("index");
        }

 
    }
}
