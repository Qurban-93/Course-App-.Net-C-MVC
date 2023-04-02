using MVCProject.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace MVCProject.Models
{
    public class Skill : BaseEntity
    {
        [Required]
        public string? Key { get; set; }
        [Required]
        public string? Value { get; set; }
        [Required]
        public int TeacherId { get; set; }
      


    }
}
