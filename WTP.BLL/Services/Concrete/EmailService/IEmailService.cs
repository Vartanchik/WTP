using System.Threading.Tasks;
using WTP.BLL.ModelsDto.Email;

namespace WTP.BLL.Services.Concrete.EmailService
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message, EmailConfigDto configuration);
    }
}
