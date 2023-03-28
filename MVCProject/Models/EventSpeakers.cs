using MVCProject.Models.Base;

namespace MVCProject.Models
{
    public class EventSpeakers:BaseEntity
    {
        public int EventId { get; set; }
        public Event? Event { get; set; }
        public int SpeakerId { get; set; }
        public Speaker? Speaker { get; set; }

    }
}
