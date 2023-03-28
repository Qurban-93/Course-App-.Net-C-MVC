using MVCProject.Models.Base;

namespace MVCProject.Models
{
    public class Notice : BaseEntity
    {
        public string Content { get; set; } = null!;
        public DateTime PublishDate { get; set; }
    }
}
