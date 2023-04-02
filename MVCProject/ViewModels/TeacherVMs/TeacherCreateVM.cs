﻿using System.ComponentModel.DataAnnotations;

namespace MVCProject.ViewModels.TeacherVMs
{
    public class TeacherCreateVM
    {
        [Required]
        public string? FullName { get; set; }
        [Required]
        public string? Post { get; set; }
        [Required]
        public string? Degree { get; set; }
        [Required]
        public string? Experience { get; set; }
        [Required]
        public string? Hobbies { get; set; }
        [Required]
        public string? Faculty { get; set; }
        [Required,DataType(DataType.EmailAddress)]
        public string? Mail { get; set; }
        [Required,DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; }
        [Required]
        public string? Skype { get; set; }
        [Required]
        public string? AboutMe { get; set; }
        [Required]
        public string? SkillsAndLevels { get; set; }    
        [Required]
        public IFormFile? Photo { get; set; }
    }
}
