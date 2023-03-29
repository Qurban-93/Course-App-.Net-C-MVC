using MVCProject.Models.Base;

namespace MVCProject.Models
{
    public class Teacher : BaseEntity
    {
        public string? FullName { get; set; }
        public string? Post { get; set; }
        public string? Degree { get; set; }
        public string? Experience { get; set; }
        public string? Hobbies { get; set; }
        public string? Faculty { get; set; }
        public string? Mail { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Skype { get; set; }
        public string? AboutMe { get; set;}
        public string? ImageUrl { get; set; }
        public List<SocialWebs>? SocialWebs { get; set; }
        public List<Skill>? Skills { get; set; }



    }
}
