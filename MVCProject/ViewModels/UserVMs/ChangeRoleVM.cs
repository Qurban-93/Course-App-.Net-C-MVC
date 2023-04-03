using Microsoft.AspNetCore.Identity;
using System.Collections;
using MVCProject.Models;

namespace MVCProject.ViewModels.UserVMs
{
    public class ChangeRoleVM
    {
        public AppUser? User { get; set; }
        public IList<string>? UserRoles { get; set; } 
        public List<IdentityRole>? Roles { get; set; }
    }
}
