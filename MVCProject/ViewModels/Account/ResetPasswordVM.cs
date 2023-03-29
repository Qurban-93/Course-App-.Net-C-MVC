using System.ComponentModel.DataAnnotations;

namespace MVCProject.ViewModels
{
    public class ResetPasswordVM
    {
        [Required, MinLength(8, ErrorMessage = "Parolun uzunlugu 8 simvoldan ibaret olmalidi !")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Required, MinLength(8, ErrorMessage = "Parolun uzunlugu 8 simvoldan ibaret olmalidi !")]
        [DataType(DataType.Password),Compare(nameof(Password))]
        public string? RepeatPassword { get; set; }

        public string Token { get; set; }
        public string userId { get; set; }
    }
}
