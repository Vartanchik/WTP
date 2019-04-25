using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.BLL.TransferModels;

namespace WTP.BLL.Services.AppUserDtoService
{
    public interface IAppUserDtoService
    {
        Task<IdentityResult> CreateAsync(AppUserDto applicationUserDto, string password);
        Task<AppUserDto> GetAsync(string id);
        Task<AppUserDto> GetByEmailAsync(string email);
        Task<IdentityResult> UpdateAsync(AppUserDto applicationUserDto);
        Task<IList<string>> GetRolesAsync(AppUserDto applicationUserDto);
        Task<bool> CheckPasswordAsync(string id, string password);
        Task<string> GetPasswordResetTokenAsync(AppUserDto applicationUserDto);
        Task<bool> IsEmailConfirmedAsync(AppUserDto applicationUserDto);
        Task<IdentityResult> ResetPasswordAsync(AppUserDto applicationUserDto, string token, string newPassword);
        Task SendEmailAsync(string email, string subject, string message);
    }
}
