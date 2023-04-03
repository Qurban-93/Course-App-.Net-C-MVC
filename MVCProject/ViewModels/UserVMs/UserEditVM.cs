namespace MVCProject.ViewModels.UserVMs
{
    public class UserEditVM
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }

        public bool Status { get; set; }
        public IList<string>? Roles { get; set; }

    }
}
