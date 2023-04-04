using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCProject.Data;
using MVCProject.Models;
using MVCProject.ViewModels.NoticeVMs;
using System.Data;

namespace MVCProject.Areas.Panel.Controllers
{
    [Area("Panel")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class NoticeController : Controller
    {
        private readonly WebAppContext _webAppContext;

        public NoticeController(WebAppContext webAppContext)
        {
            _webAppContext = webAppContext;
        }

        public IActionResult Index() { return View(_webAppContext.Notices.ToList()); }

        public IActionResult Create() { return View(); }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(NoticeCreateVM noticeCreateVM)
        {
            if (noticeCreateVM == null) return NotFound();
            if (!ModelState.IsValid) return View(noticeCreateVM);

            Notice notice = new();
            notice.PublishDate = DateTime.Now;
            notice.Content = noticeCreateVM.Content;

            _webAppContext.Notices.Add(notice);
            _webAppContext.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();
            Notice notice = _webAppContext.Notices.FirstOrDefault(n => n.Id == id);
            if (notice == null) return NotFound();
            return View(notice);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(int? id, NoticeCreateVM noticeCreateVM)
        {
            if (id == null || id == 0) return NotFound();
            if (noticeCreateVM == null) return NotFound();
            if (!ModelState.IsValid) return View(noticeCreateVM);

            Notice notice = _webAppContext.Notices.FirstOrDefault(n => n.Id == id);
            if (notice == null) return NotFound();

            notice.Content = noticeCreateVM.Content;
            _webAppContext.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();
            Notice notice = _webAppContext.Notices.FirstOrDefault(n => n.Id == id);
            if (notice == null) return NotFound();

            _webAppContext.Notices.Remove(notice);
            _webAppContext.SaveChanges();

            return RedirectToAction("index");
        }
    }
}
