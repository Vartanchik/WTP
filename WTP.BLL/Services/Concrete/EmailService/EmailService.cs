using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace WTP.BLL.Services.Concrete.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MailMessage(_configuration["Email:Email"], email);
            emailMessage.Subject = subject;
            emailMessage.IsBodyHtml = true;
            emailMessage.Body = message;

            using (var client = new SmtpClient())
            {
                client.Host = _configuration["Email:Host"];
                client.Port = int.Parse(_configuration["Email:Port"]);
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(_configuration["Email:Email"], _configuration["Email:Password"]);
                await client.SendMailAsync(emailMessage);
            }
        }
    }
}
