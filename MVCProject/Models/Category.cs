using MVCProject.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace MVCProject.Models
{
    public class Category : BaseEntity
    {
        [Required,MaxLength(40),MinLength(3)]
        public string? CategoryName { get; set; }
        public List<CourseCategory>? CourseCategories { get; set; }
    }
}
