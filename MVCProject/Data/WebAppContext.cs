using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVCProject.Models;

namespace MVCProject.Data
{
    public class WebAppContext : IdentityDbContext<AppUser>
    {
        public WebAppContext(DbContextOptions<WebAppContext> options): base(options) { }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Notice> Notices { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<NoticeInfo> NoticeInfos { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Speaker> Speakers { get; set; }
        public DbSet<EventSpeakers> EventsSpeakers { get; set;}
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<SocialWebs> SocialWebs { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Settings> Settings { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Settings>().HasIndex(s=>s.Key).IsUnique();

            
            base.OnModelCreating(builder);
        }



    }
}
