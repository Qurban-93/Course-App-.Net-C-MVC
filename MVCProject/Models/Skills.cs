using MVCProject.Models.Base;

namespace MVCProject.Models
{
    public class Skills : BaseEntity
    {
        public string? Language { get; set; }
        public string? Development { get; set; }
        public string? TeamLider { get; set; }
        public string? Design { get; set; }
        public string? Innovation { get; set; }
        public string? Communication { get; set; }
        public int TeacherId { get; set; }


    }
}
