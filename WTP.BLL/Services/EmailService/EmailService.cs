using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using WTP.BLL.DTOs.ServicesDTOs;
using WTP.Logging;

namespace WTP.BLL.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly ILog _log;

        public EmailService(ILog log)
        {
            _log = log;
        }

        public async Task SendEmailAsync(string email, string subject, string message, EmailConfigDto configuration)
        {
            try
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
            // TODO: Add concrete exceptions
            catch (Exception ex)
            {
                _log.Error($"{this.ToString()} - error message:{ex.Message}");
            }
        }
    }
}
