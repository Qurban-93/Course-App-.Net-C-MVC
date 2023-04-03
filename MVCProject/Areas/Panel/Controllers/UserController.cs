using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCProject.Models;
using MVCProject.ViewModels.UserVMs;

namespace WebApplication3.Areas.Admin.Controllers
{
    [Area("Panel")]
    public class UserController : Controller
    {
        UserManager<AppUser> _userManager;
        RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index(string search)
        {
            List<AppUser> allUsers = await _userManager.Users.ToListAsync();
            List<AppUser> admins = new();
            UserIndexVM userIndexVM = new UserIndexVM();
            userIndexVM.AllUsers = allUsers;
            foreach (AppUser user in allUsers)
            {

                IList<string> roles = await _userManager.GetRolesAsync(user);
                if (roles.Any(r => r.ToLower().Contains("admin")))
                {
                    admins.Add(user);

                }
            }

            userIndexVM.Admins = admins;

            if (search != null)
            {
                List<AppUser> user = _userManager.Users.Where(u => u.UserName.Contains(search)).ToList();
                userIndexVM.AllUsers = user;
                return View(userIndexVM);
            }

            return View(userIndexVM);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();
            AppUser? user = await _userManager.FindByIdAsync(id);
            if (user == null) return View();
            UserDetailsVM userDetailsVM = new UserDetailsVM();
            userDetailsVM.UserName = user.UserName;
            userDetailsVM.UserRoles = await _userManager.GetRolesAsync(user);

            return View(userDetailsVM);
        }

        public async Task<IActionResult> EditRole(string id)
        {
            if (id == null) return NotFound();
            AppUser? user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            ChangeRoleVM changeRoleVM = new ChangeRoleVM();
            changeRoleVM.UserRoles = await _userManager.GetRolesAsync(user);
            changeRoleVM.Roles = _roleManager.Roles.ToList();
            changeRoleVM.User = user;

            return View(changeRoleVM);
        }


        [HttpPost]
        public async Task<IActionResult> EditRole(string id, List<string> roles)
        {
            if (id == null) return NotFound();
            if (roles == null) return BadRequest();
            AppUser? user = await _userManager.FindByIdAsync(id);
            if (user == null) return BadRequest();
            var oldRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, oldRoles);
            await _userManager.AddToRolesAsync(user, roles);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ChangeStatus(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();
            AppUser? user = await _userManager.FindByIdAsync(id);
            if (user == null) return BadRequest();
            if (user.Active)
            {
                user.Active = false;
            }
            else
            {
                user.Active = true;
            }
            await _userManager.UpdateAsync(user);
            return RedirectToAction(nameof(Index));
        }
    }
}
