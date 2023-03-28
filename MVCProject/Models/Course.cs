using MVCProject.Models.Base;

namespace MVCProject.Models
{
    public class Course : BaseEntity
    {
        public string? CourseName { get; set; }
        public DateTime StartDate { get; set; }
        public string? Duration { get; set; }
        public string? ClassDuration { get; set; }
        public string? SkillLevel { get; set; }
        public string? Language { get; set; }
        public string? StudentsCount { get; set; }
        public double? CoursePrice { get; set; }
        public string? Description { get; set; }
        public string? AboutCourse { get; set; }
        public string? HowToApply { get; set; }
        public string? Certification { get; set; }





    }
}
