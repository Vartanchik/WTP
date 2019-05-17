using System.Threading.Tasks;
using WTP.BLL.Dto.Email;

namespace WTP.BLL.Services.EmailService
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message, EmailConfigDto configuration);
    }
}
