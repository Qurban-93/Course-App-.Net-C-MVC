using System.ComponentModel.DataAnnotations;

namespace MVCProject.ViewModels.EventVMs
{
    public class EventEditVM
    {
        [Required]
        public string? EventName { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }
        [Required]
        public string? Adress { get; set; }
        [Required]
        public string? Description { get; set; }    
        public string? ImageUrl { get; set; }
        public IFormFile? Photo { get; set; }
        public int[]? SpeakersId { get; set; }
    }
}
