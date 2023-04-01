using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using MimeKit;
using MimeKit.Text;
using MVCProject.Data;
using MVCProject.Extencions;
using MVCProject.Interfaces;
using MVCProject.Models;
using MVCProject.ViewModels.CourseVMs;
using MVCProject.ViewModels.EventVMs;
using MVCProject.ViewModels.SliderVMs;
using NuGet.Packaging;

namespace MVCProject.Areas.Panel.Controllers
{
    [Area("Panel")]
    public class EventController : Controller
    {
        private readonly WebAppContext _appContext;
        private readonly IWebHostEnvironment _env;
        private readonly ISendEmailService _sendEmailService;

        public EventController(WebAppContext appContext, IWebHostEnvironment env, ISendEmailService sendEmailService)
        {
            _appContext = appContext;
            _env = env;
            _sendEmailService = sendEmailService;
        }

        public IActionResult Index()
        {
            List<Event> events = _appContext.Events.Include(e => e.EventSpeakers).ThenInclude(es => es.Speaker).ToList();
            return View(events);
        }

        public IActionResult Create()
        {
            ViewBag.Speakers = new SelectList(_appContext.Speakers.ToList(), "Id", "FullName");
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(EventCreateVM eventCreateVM)
        {
            ViewBag.Speakers = new SelectList(_appContext.Speakers.ToList(), "Id", "FullName");
            if (!ModelState.IsValid) return View();
            if (_appContext.Events.Any(e => e.EventName == eventCreateVM.EventName))
            {
                ModelState.AddModelError("EventName", "Bu adla event movcuddur !");
                return View(eventCreateVM);
            }
            if (DateTime.Now > eventCreateVM.StartTime)
            {
                ModelState.AddModelError("StartTime", "Baslama vaxti kecmish vaxt ola bilmez !");
                return View(eventCreateVM);
            }
            if (eventCreateVM.StartTime > eventCreateVM.EndTime)
            {
                ModelState.AddModelError("EndTime", "Bitme vaxti Baslamadan kecmish vaxt ola bilmez !");
                return View(eventCreateVM);
            }

            if (!eventCreateVM.Photo.CheckImageType())
            {
                ModelState.AddModelError("Photo", "Ancaq Image Elave ele !");
                return View();
            }
            if (eventCreateVM.Photo.CheckImageSize(5))
            {
                ModelState.AddModelError("Photo", "Sekilin hecimi 5 Mgb artiq ola bilmez !");
                return View();
            }
            List<EventSpeakers> eventSpeakers = new List<EventSpeakers>();

            foreach (var item in eventCreateVM.EventSpeakersId)
            {
                EventSpeakers eventSpeaker = new();
                eventSpeaker.SpeakerId = item;
                eventSpeakers.Add(eventSpeaker);
            }


            Event newEvent = new Event();
            newEvent.ImageUrl = eventCreateVM.Photo.SaveImage(_env, "img", "event");
            newEvent.EventName = eventCreateVM.EventName;
            newEvent.EventSpeakers = eventSpeakers;
            newEvent.StartTime = eventCreateVM.StartTime;
            newEvent.EndTime = eventCreateVM.EndTime;
            newEvent.Adress = eventCreateVM.Adress;
            newEvent.Description = eventCreateVM.Description;

            _appContext.Events.Add(newEvent);
            await _appContext.SaveChangesAsync();

            List<AppUser> users = await _appContext.Users.Where(u => u.Subscribe == true).ToListAsync();

            InternetAddressList internetAddresses = new InternetAddressList();

            foreach (AppUser user in users)
            {
                internetAddresses.Add(MailboxAddress.Parse(user.Email));
            }

            MimeMessage email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("qurban231293@gmail.com"));
            email.To.AddRange(internetAddresses);
            email.Subject = "New Event In EduHome";
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = "we invite you to visit our event which " +
                "will take place next month. for more information, " +
                "you can visit our website www.eduhome.com/events"
            };

            _sendEmailService.SendEmail("qurban231293@gmail.com", "olszimzdwkxyjwwz", "smtp.gmail.com", email);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Speakers = new SelectList(_appContext.Speakers.ToList(), "Id", "FullName");
            if (id == null || id == 0) return NotFound();

            Event? eventE = await _appContext.Events.FirstOrDefaultAsync(e => e.Id == id);
            if (eventE == null) return NotFound();


            EventEditVM eventEditVM = new EventEditVM();
            eventEditVM.Adress = eventE.Adress;
            eventEditVM.EventName = eventE.EventName;
            eventEditVM.StartTime = eventE.StartTime;
            eventEditVM.EndTime = eventE.EndTime;
            eventEditVM.Description = eventE.Description;
            eventEditVM.ImageUrl = eventE.ImageUrl;

            return View(eventEditVM);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(int? id, EventEditVM? eventEditVM)
        {
            ViewBag.Speakers = new SelectList(_appContext.Speakers.ToList(), "Id", "FullName");
            if (id == null || id == 0)  return NotFound(); 

            Event eventE = await _appContext.Events.FirstOrDefaultAsync(e => e.Id == id);

            if (!ModelState.IsValid)
            {
                eventEditVM.ImageUrl = _appContext.Events.FirstOrDefault(e => e.Id == id).ImageUrl;
                return View(eventEditVM);
            }
            if (eventEditVM.Photo != null)
            {
                if (!eventEditVM.Photo.CheckImageType())
                {
                    ModelState.AddModelError("Photo", "Ancaq Image Elave ele !");
                    return View();
                }
                if (eventEditVM.Photo.CheckImageSize(5))
                {
                    ModelState.AddModelError("Photo", "Sekilin hecimi 5 Mgb artiq ola bilmez !");
                    return View();
                }

                string fullPath = Path.Combine(_env.WebRootPath, "img", "event", eventE.ImageUrl);

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                else
                {
                    ModelState.AddModelError("Photo", "Kohne sekil tapilmadi yeniden cehd edin !");
                    eventEditVM.ImageUrl = eventE.ImageUrl;
                    return View(eventEditVM);
                }

                eventE.ImageUrl = eventEditVM.Photo.SaveImage(_env, "img", "event");
            }
            if (eventEditVM.SpeakersId != null)
            {
                List<EventSpeakers> eventSpeakers = new List<EventSpeakers>();

                foreach (var item in eventEditVM.SpeakersId)
                {
                    EventSpeakers eventSpeaker = new();
                    eventSpeaker.SpeakerId = item;
                    eventSpeakers.Add(eventSpeaker);
                }
                eventE.EventSpeakers.Clear();
                eventE.EventSpeakers = eventSpeakers;
            }
            if (_appContext.Events.Any(e => e.EventName == eventEditVM.EventName && e.Id != id))
            {
                ModelState.AddModelError("EventName", "Bu adla Event artiq var !");
                eventEditVM.ImageUrl = _appContext.Events.FirstOrDefault(e => e.Id == id).ImageUrl;
                return View(eventEditVM);
            }
            if (DateTime.Now > eventEditVM.StartTime)
            {
                ModelState.AddModelError("StartTime", "Baslama vaxti kecmish vaxt ola bilmez !");
                eventEditVM.ImageUrl = _appContext.Events.FirstOrDefault(e => e.Id == id).ImageUrl;

                return View(eventEditVM);
            }
            if (eventEditVM.StartTime > eventEditVM.EndTime)
            {
                ModelState.AddModelError("EndTime", "Bitme vaxti Baslamadan kecmish vaxt ola bilmez !");
                eventEditVM.ImageUrl = _appContext.Events.FirstOrDefault(e => e.Id == id).ImageUrl;
                return View(eventEditVM);
            }


            eventE.EventName = eventEditVM.EventName;
            eventE.Adress = eventEditVM.Adress;
            eventE.Description = eventEditVM.Description;
            eventE.EndTime = eventEditVM.EndTime;
            eventE.StartTime = eventEditVM.StartTime;

            await _appContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();
            Event existEvent = await _appContext.Events.FirstOrDefaultAsync(x => x.Id == id);

            _appContext.Events.Remove(existEvent);
            await _appContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
