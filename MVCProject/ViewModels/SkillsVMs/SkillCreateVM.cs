using System.ComponentModel.DataAnnotations;

namespace MVCProject.ViewModels.SkillsVMs
{
    public class SkillCreateVM
    {
        [Required]
        public string? Key { get; set; }
        [Required,MaxLength(3)]
        public string? Value { get; set; }
        [Required]
        public int TeacherId { get; set; }
    }
}
