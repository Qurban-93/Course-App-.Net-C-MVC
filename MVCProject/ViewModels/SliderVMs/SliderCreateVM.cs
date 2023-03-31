using System.ComponentModel.DataAnnotations;

namespace MVCProject.ViewModels.SliderVMs
{
    public class SliderCreateVM
    {
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public string? ButtonUrl { get; set; }
        [Required]
        public IFormFile? Photo { get; set; }
    }
}
