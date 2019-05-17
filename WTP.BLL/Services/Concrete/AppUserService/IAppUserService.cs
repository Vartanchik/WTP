using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.BLL.Models.AppUserModels;

namespace WTP.BLL.Services.Concrete.AppUserService
{
    public interface IAppUserService
    {
        Task<OperationResult> CreateAsync(AppUserModel model, string password);
        Task<OperationResult> UpdateAsync(AppUserModel model);
        Task<OperationResult> DeleteAsync(int userId);
        Task<AppUserModel> FindByIdAsync(int userId);
        Task<AppUserModel> FindByIdAsync(string userId);
        Task<AppUserModel> GetByEmailAsync(string userEmail);
        Task<AppUserModel> GetByNameAsync(string userName);
        Task<IList<string>> GetRolesAsync(AppUserModel model);
        Task<bool> CheckPasswordAsync(int userId, string password);
        Task<bool> IsEmailConfirmedAsync(int userId);
        Task<OperationResult> ResetPasswordAsync(ResetPasswordModel model);
        Task<OperationResult> ConfirmEmailAsync(int userId, string token);
        Task<OperationResult> ChangePasswordAsync(ChangePasswordModel model);
        Task<string> GeneratePasswordResetTokenAsync(AppUserModel model);
        Task<string> GenerateEmailConfirmationTokenAsync(AppUserModel model);
    }
}
