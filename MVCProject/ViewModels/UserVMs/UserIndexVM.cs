using MVCProject.Models;

namespace MVCProject.ViewModels.UserVMs
{
    public class UserIndexVM
    {
        public List<AppUser> AllUsers { get; set; }
        public List<AppUser> Admins { get; set; }

    }
}
