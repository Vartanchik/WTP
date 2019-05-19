using System.Threading.Tasks;
using WTP.BLL.DTOs.ServicesDTOs;

namespace WTP.BLL.Services.EmailService
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message, EmailConfigDto dto);
    }
}
