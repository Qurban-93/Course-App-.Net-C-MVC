using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCProject.Data;
using MVCProject.Models;
using MVCProject.ViewModels.SkillsVMs;

namespace MVCProject.Areas.Panel.Controllers
{
    [Area("Panel")]
    public class SkillController : Controller
    {
        private readonly WebAppContext _webAppContext;

        public SkillController(WebAppContext webAppContext)
        {
            _webAppContext = webAppContext;
        }

        public IActionResult Index()
        {
            List<Teacher> list = _webAppContext.Teachers.Include(t=>t.Skills).ToList();
            return View(list);
        }

        public IActionResult Create(int? id)
        {
            if(id == null || id == 0) return NotFound();
            SkillCreateVM skillCreateVM = new SkillCreateVM();
            skillCreateVM.TeacherId = (int)id;
            return View(skillCreateVM);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(int? id,SkillCreateVM skillCreateVM)
        {
            if (id == null || id == 0) return NotFound();
            if (skillCreateVM == null) return NotFound();
            if (!ModelState.IsValid) return View(skillCreateVM);

            foreach (var item in skillCreateVM.Value)
            {
                if (!char.IsDigit(item))
                {
                    ModelState.AddModelError("Value", "Yalniz reqem daxil olunmalidi!");
                    return View(skillCreateVM);
                }
            }
            if (skillCreateVM.Value.Length == 3 && skillCreateVM.Value != "100")
            {
                ModelState.AddModelError("Value", "Sehvlik var! max value : 100!");
                return View(skillCreateVM);
            }

            skillCreateVM.Value = string.Concat(skillCreateVM.Value, "%");

            Skill skill = new Skill();
            skill.Value = skillCreateVM.Value;
            skill.Key = skillCreateVM.Key;

            Teacher teacher = _webAppContext.Teachers.Include(t=>t.Skills).FirstOrDefault(t=>t.Id== id);
            if(teacher==null)return NotFound();
            if(teacher.Skills.Count != 0 || teacher.Skills != null)
            {
                teacher.Skills.Add(skill);
            }
            else
            {
                List<Skill> list = new List<Skill>();
                list.Add(skill);
                teacher.Skills = list;
            }

            _webAppContext.SaveChanges();
            return RedirectToAction("index");

        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();
            Skill skill = _webAppContext.Skills.FirstOrDefault(t => t.Id == id);
            if (skill == null) return NotFound();
            SkillEditVM skillEditVM = new SkillEditVM();    
            skillEditVM.Key = skill.Key;
            skillEditVM.Value = skill.Value.Substring(0,skill.Value.Length-1);
            
            return View(skillEditVM);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(int? id,SkillEditVM skillEditVM)
        {
            if (id == null || id == 0) return NotFound();         
            if (skillEditVM == null) return NotFound();
            if (!ModelState.IsValid) return View(skillEditVM);
         
            foreach (var item in skillEditVM.Value) 
            {
                if (!char.IsDigit(item))
                {
                    ModelState.AddModelError("Value","Yalniz reqem daxil olunmalidi!");
                    return View(skillEditVM);
                }
            }
            if(skillEditVM.Value.Length == 3 && skillEditVM.Value != "100") 
            {
                ModelState.AddModelError("Value", "Sehvlik var! max value : 100!");
                return View(skillEditVM);
            }
            skillEditVM.Value = string.Concat(skillEditVM.Value, "%");

            Skill existSkill = _webAppContext.Skills.FirstOrDefault(skill=> skill.Id == id);
            if (existSkill == null) return NotFound();
            existSkill.Value = skillEditVM.Value;
            existSkill.Key= skillEditVM.Key;
            _webAppContext.SaveChanges();


            return RedirectToAction("index");
        }

        public IActionResult Delete(int? id)
        {
            if(id == null || id == 0) return NotFound();
            Skill skill= _webAppContext.Skills.FirstOrDefault(sk=> sk.Id == id);
            if (skill == null) return NotFound();
            _webAppContext.Skills.Remove(skill);
            _webAppContext.SaveChanges();
            return RedirectToAction("index");

        }
    }
}
