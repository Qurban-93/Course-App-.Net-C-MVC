using MVCProject.Models;

namespace MVCProject.ViewModels
{
    public class HomeVM
    {
        public List<Slider>? Sliders { get; set; }
        public List<Notice>? Notices { get; set; }
        public List<NoticeInfo>? NoticesInfo { get; set;}
        public List<Course>? Courses { get; set; }
        public List<Event>? Events { get; set; } 
        public List<Blog>? Blogs { get; set; }
    }
}
