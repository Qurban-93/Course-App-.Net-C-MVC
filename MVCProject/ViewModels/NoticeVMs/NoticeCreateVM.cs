

using System.ComponentModel.DataAnnotations;

namespace MVCProject.ViewModels.NoticeVMs
{
    public class NoticeCreateVM
    {
        [Required]
        public string? Content { get; set; }       
    }
}
