using MVCProject.Models.Base;

namespace MVCProject.Models
{
    public class SocialWebs : BaseEntity
    {
        public string? Facebook { get; set; }
        public string? Instagram { get; set; }
        public string? LinkedIn { get; set; }
        public string? Twitter { get; set; }
        public int TeacherId { get; set; }
        public Teacher? Teacher { get; set;}

    }
}
