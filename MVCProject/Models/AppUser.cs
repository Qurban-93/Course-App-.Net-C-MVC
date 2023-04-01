using Microsoft.AspNetCore.Identity;

namespace MVCProject.Models
{
    public class AppUser:IdentityUser
    {
        public string? FullName { get; set; }
        public bool Subscribe { get; set; }
        public List<Comment>? Comments { get; set; }
    }
}
