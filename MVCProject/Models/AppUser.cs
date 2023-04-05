using Microsoft.AspNetCore.Identity;

namespace MVCProject.Models
{
    public class AppUser:IdentityUser
    {
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public string? MobileNumber { get; set; }
        public string? ImageUrl { get; set; }
        public bool Active { get; set; }
        public bool Subscribe { get; set; }
        public List<Comment>? Comments { get; set; }
    }
}
