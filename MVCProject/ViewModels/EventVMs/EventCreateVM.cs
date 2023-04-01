using MVCProject.Models;
using System.ComponentModel.DataAnnotations;

namespace MVCProject.ViewModels.EventVMs
{
    public class EventCreateVM
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
        [Required]
        public IFormFile Photo { get; set; }
        [Required]
        public int[] EventSpeakersId { get; set; }
      
    }
}
