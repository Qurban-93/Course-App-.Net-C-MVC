using Microsoft.AspNetCore.Identity;

namespace MVCProject.ViewModels.UserVMs
{
    public class UserDetailsVM
    {
        public string UserName { get; set;}
        public string Email { get; set;}
        public IList<string> UserRoles { get; set;}
        public List<IdentityRole> AllRoles { get; set;}
    }
}
