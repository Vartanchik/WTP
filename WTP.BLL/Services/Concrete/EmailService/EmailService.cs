using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace WTP.BLL.Services.Concrete.EmailService
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MailMessage("avg0test0@gmail.com", email);
            emailMessage.Subject = subject;
            emailMessage.IsBodyHtml = true;
            emailMessage.Body = message;

            using (var client = new SmtpClient())
            {
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("avg0test0@gmail.com", "TeSt159357");
                await client.SendMailAsync(emailMessage);
            }
        }
    }
}
