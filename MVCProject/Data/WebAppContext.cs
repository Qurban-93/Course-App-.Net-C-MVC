using Microsoft.EntityFrameworkCore;

namespace MVCProject.Data
{
    public class WebAppContext : DbContext
    {
        public WebAppContext(DbContextOptions<WebAppContext> options): base(options) { }
        

        
    }
}
