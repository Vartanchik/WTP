using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using WTP.BLL.ModelsDto.Email;

namespace WTP.BLL.Services.Concrete.EmailService
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string email, string subject, string message, EmailConfigModel configuration)
        {
            var emailMessage = new MailMessage(configuration.Email, email)
            {
                Subject = subject,
                IsBodyHtml = true,
                Body = message
            };

            using (var client = new SmtpClient())
            {
                client.Host = configuration.Host;
                client.Port = int.Parse(configuration.Port);
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(configuration.Email, configuration.Password);
                await client.SendMailAsync(emailMessage);
            }
        }
    }
}
