using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MVCProject.Interfaces;

namespace MVCProject.Service
{
    public class SendEmailService : ISendEmailService
    {
       
        public void SendEmail(string fromEmail, string password,string mailHost,MimeMessage email)
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Connect(mailHost, 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(fromEmail, password);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
