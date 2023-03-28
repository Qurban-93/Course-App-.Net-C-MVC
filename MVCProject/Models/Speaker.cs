using MVCProject.Models.Base;

namespace MVCProject.Models
{
    public class Speaker:BaseEntity
    {
        public string? FullName { get; set; }
        public string? ImageUrl { get; set; }
        public string? Professions { get; set; }
        public List<EventSpeakers>? EventsSpeakers { get; set;}

    }
}
