using Microsoft.AspNetCore.Identity;
using MVCProject.Data;
using MVCProject.Models;


namespace MVCProject.Service
{
    public class LayoutService
    {
        private readonly WebAppContext _context;
        private readonly SignInManager<AppUser> _signInManager;
        public LayoutService(WebAppContext context, SignInManager<AppUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }

        

        public List<Settings> GetSettings()
        {
            List<Settings> settings = _context.Settings.ToList();
            return settings;
        }

        public bool GetUserInfo()
        {
            
           
            return true;
        }
    }
}
