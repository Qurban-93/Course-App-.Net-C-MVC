using MVCProject.Models.Base;

namespace MVCProject.Models
{
    public class Comment : BaseEntity
    {
        public AppUser? User { get; set; }
        public string? AppUserId { get; set; }
        public int BlogId { get; set; }
        public Blog? Blog { get; set; }
        public string Content { get; set; } = null!;
        public DateTime Date { get; set; }
    }
}
