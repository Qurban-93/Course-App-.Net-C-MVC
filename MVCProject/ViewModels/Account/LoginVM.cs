using System.ComponentModel.DataAnnotations;

namespace MVCProject.ViewModels
{
    public class LoginVM
    {
        [Required,MaxLength(50)]
        public string UserNameOrEmail { get; set; }
        [Required,DataType(DataType.Password),MinLength(8)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
