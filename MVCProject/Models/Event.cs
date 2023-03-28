using MVCProject.Models.Base;

namespace MVCProject.Models
{
    public class Event: BaseEntity
    {
        public string? EventName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Adress { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public List<EventSpeakers>? EventSpeakers { get; set; }

    }
}
