using MVCProject.Models.Base;

namespace MVCProject.Models
{

    public class Settings : BaseEntity
    {
        public string? Key { get; set; }
        public string? Value { get; set; }
    }
}
