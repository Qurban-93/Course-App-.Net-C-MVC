using MVCProject.Models.Base;

namespace MVCProject.Models
{
    public class Tag : BaseEntity
    {
        public string? Key { get; set; }
        public string? Value { get; set; }
    }
}
