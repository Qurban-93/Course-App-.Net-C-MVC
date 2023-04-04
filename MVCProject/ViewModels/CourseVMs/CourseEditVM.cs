﻿using System.ComponentModel.DataAnnotations;

namespace MVCProject.ViewModels.CourseVMs
{
    public class CourseEditVM
    {
        [Required]
        public string? CourseName { get; set; }
        [Required]
        public int[]? CategoriesId { get; set; }
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
        public string? ImageUrl { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
