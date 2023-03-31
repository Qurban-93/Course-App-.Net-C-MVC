using System.ComponentModel.DataAnnotations;

namespace MVCProject.ViewModels.CourseVMs
{
    public class CourseCreateVM
    {
        [Required]
        public string? CourseName { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public string? Duration { get; set; }
        [Required]
        public string? ClassDuration { get; set; }
        [Required]
        public string? SkillLevel { get; set; }
        [Required]
        public string? Language { get; set; }
        [Required]
        public string? StudentsCount { get; set; }
        [Required]
        public double? CoursePrice { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public string? AboutCourse { get; set; }
        [Required]
        public string? HowToApply { get; set; }
        [Required]
        public string? Certification { get; set; }
        [Required]
        public IFormFile? Photo { get; set; }
    }
}
