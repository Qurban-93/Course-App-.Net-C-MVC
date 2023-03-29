using MVCProject.Data;
using MVCProject.Models;

namespace MVCProject.Service
{
    public class LayoutService
    {
        private readonly WebAppContext _context;
        public LayoutService(WebAppContext context)
        {
            _context = context;
        }

        public List<Settings> GetSettings()
        {
            List<Settings> settings = _context.Settings.ToList();
            return settings;
        }
    }
}
