using MVCProject.Models.Base;

namespace MVCProject.Models
{
    public class Slider : BaseEntity
    {
        public string? Title { get; set; }
        public string?  Description { get; set; }
        public string? ButtonUrl { get; set; }
        public string?  ImageUrl { get; set; }
    }
}
