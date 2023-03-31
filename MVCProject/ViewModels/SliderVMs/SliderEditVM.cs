using System.ComponentModel.DataAnnotations;

namespace MVCProject.ViewModels.SliderVMs
{
    public class SliderEditVM
    {
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public string? ButtonUrl { get; set; }       
        public string? ImageUrl { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
