using System.ComponentModel.DataAnnotations;

namespace MVCProject.ViewModels
{
    public class RegisterVM
    {
        [Required,MaxLength(50)]
        public string? FullName { get; set; }
        [Required,MaxLength(50)]
        public string? UserName { get; set; }
        [Required, DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Required, DataType(DataType.Password)]
        public string? Password { get; set; }
        [Required,Compare("Password"),DataType(DataType.Password)]
        public string? RepeatPassword { get; set; }
        public bool RememmberMe { get; set; }          
    }
}
