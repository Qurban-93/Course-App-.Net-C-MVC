using System.ComponentModel.DataAnnotations;

namespace MVCProject.ViewModels.SkillsVMs
{
    public class SkillCreateVM
    {
        [Required]
        public string? Key { get; set; }
        [Required]
        public string? Value { get; set; }
        [Required]
        public int TeacherId { get; set; }
    }
}
