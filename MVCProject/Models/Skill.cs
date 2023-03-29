using MVCProject.Models.Base;

namespace MVCProject.Models
{
    public class Skill : BaseEntity
    {
        public string? Key { get; set; }
        public string? Value { get; set; }
        public int TeacherId { get; set; }
      


    }
}
