using MVCProject.Models.Base;

namespace MVCProject.Models
{
    public class Blog : BaseEntity
    {
        public string? ImageUrl { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Author { get; set; }
        public DateTime? PublishDate { get; set; }
        public List<Comment>? Comments { get; set; }
    }
}
