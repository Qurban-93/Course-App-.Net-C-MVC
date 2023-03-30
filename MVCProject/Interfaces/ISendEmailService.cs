using MimeKit;

namespace MVCProject.Interfaces
{
    public interface ISendEmailService
    {
        public void SendEmail(string fromEmail,string password,string mailHost, MimeMessage mail);
    }
}
