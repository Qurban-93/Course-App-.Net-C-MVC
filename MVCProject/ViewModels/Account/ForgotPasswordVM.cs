using System.ComponentModel.DataAnnotations;

namespace MVCProject.ViewModels
{
    public class ForgotPasswordVM
    {
        [Required,DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
    }
}
