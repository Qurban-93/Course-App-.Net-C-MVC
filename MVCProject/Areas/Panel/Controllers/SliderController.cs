using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCProject.Data;
using MVCProject.Extencions;
using MVCProject.Models;
using MVCProject.ViewModels.SliderVMs;

namespace MVCProject.Areas.Panel.Controllers
{
    [Area("Panel")]
    public class SliderController : Controller
    {
        private readonly WebAppContext _appContext;
        private readonly IWebHostEnvironment _env;

        public SliderController(WebAppContext appContext,IWebHostEnvironment env) 
        {
            _appContext= appContext;
            _env= env;
        }

        public IActionResult Index()
        {
            return View(_appContext.Sliders.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SliderCreateVM sliderCreateVM)
        {
            
            if(!ModelState.IsValid) return View();
                  
            if (!sliderCreateVM.Photo.CheckImageType())
            {
                ModelState.AddModelError("Photo", "Ancaq Image Elave ele !");
                return View();
            }
            if (sliderCreateVM.Photo.CheckImageSize(5))
            {
                ModelState.AddModelError("Photo", "Sekilin hecimi 5 Mgb artiq ola bilmez !");
                return View();
            }
            Slider newSlider = new();
            newSlider.ImageUrl = sliderCreateVM.Photo.SaveImage(_env, "img","slider");
            newSlider.Description = sliderCreateVM.Description;
            newSlider.Title= sliderCreateVM.Title;
            newSlider.ButtonUrl= sliderCreateVM.ButtonUrl;

            _appContext.Sliders.Add(newSlider);
            _appContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();
            Slider? slider = _appContext.Sliders.FirstOrDefault(x => x.Id == id);
            if (slider == null) return NotFound();
            SliderEditVM sliderEditVM = new SliderEditVM();
            sliderEditVM.Title = slider.Title;
            sliderEditVM.Description = slider.Description;
            sliderEditVM.ButtonUrl= slider.ButtonUrl;
            sliderEditVM.ImageUrl = slider.ImageUrl;
            return View(sliderEditVM);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(int? id,SliderEditVM sliderEditVM)
        {
            if(sliderEditVM == null) return View();
            if (!ModelState.IsValid)
            {
                sliderEditVM.ImageUrl = _appContext.Sliders.FirstOrDefault(y => y.Id == id).ImageUrl;
                return View(sliderEditVM); 
            }
          
            Slider? slider = _appContext.Sliders.FirstOrDefault(s=>s.Id == id);
            if (slider == null) return NotFound();


            if(sliderEditVM.Photo != null)
            {
                if (!sliderEditVM.Photo.CheckImageType())
                {
                    ModelState.AddModelError("Photo", "Ancaq Image Elave ele !");
                    return View();
                }
                if (sliderEditVM.Photo.CheckImageSize(5))
                {
                    ModelState.AddModelError("Photo", "Sekilin hecimi 5 Mgb artiq ola bilmez !");
                    return View();
                }

                string fullPath = Path.Combine(_env.WebRootPath, "img","slider", slider.ImageUrl);

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                else
                {
                    ModelState.AddModelError("ImageUrl", "Kohne sekil tapilmadi yeniden cehd edin !");
                    return View(sliderEditVM);
                }

                slider.ImageUrl = sliderEditVM.Photo.SaveImage(_env,"img","slider");
            }

            slider.Description = sliderEditVM.Description;
            slider.Title = sliderEditVM.Title;
            slider.ButtonUrl= sliderEditVM.ButtonUrl;

            _appContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();

            Slider existSlider = _appContext.Sliders.FirstOrDefault(s => s.Id == id);
            string fullPath = Path.Combine(_env.WebRootPath, "img", "slider", existSlider.ImageUrl);

            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }

            _appContext.Sliders.Remove(existSlider);
            _appContext.SaveChanges();
            
            return RedirectToAction(nameof(Index));
        }


    }
}
