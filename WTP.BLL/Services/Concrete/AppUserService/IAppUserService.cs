using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WTP.BLL.DTOs.AppUserDTOs;
using WTP.BLL.DTOs.ServicesDTOs;
using WTP.BLL.Shared;
using WTP.DAL.Entities.AppUserEntities;

namespace WTP.BLL.Services.Concrete.AppUserService
{
    public interface IAppUserService
    {
        Task<AppUserDto> GetByIdAsync(int userId);
        Task<IdentityResult> CreateAsync(AppUserDto appUserDto, string password, int? adminId=null);
        Task<AppUserDto> GetByEmailAsync(string email);
        Task<AppUserDto> GetByNameAsync(string userName);
        Task<IdentityResult> UpdateAsync(AppUserDto appUserDto, int? adminId=null);
        Task<IList<string>> GetRolesAsync(AppUserDto appUserDto);
        Task<bool> CheckPasswordAsync(int userId, string password);
        Task<bool> IsEmailConfirmedAsync(int userId);
        Task<string> GeneratePasswordResetTokenAsync(AppUserDto appUserDto);
        Task<IdentityResult> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
        Task<IdentityResult> ChangePasswordAsync(ChangePasswordDto changePasswordDto);
        Task<string> GenerateEmailConfirmationTokenAsync(AppUserDto appUserDto);
        Task<AppUserDto> FindByIdAsync(string userId);
        Task<IdentityResult> ConfirmEmailAsync(int userId, string token);

        Task DeleteAccountAsync(int userId);
        Task<string> CreateRestoreAccountToken(int userId);
        Task<bool> RestoreAccountAsync(int userId, string token);

        Task<IdentityResult> CreateAdminAsync(AppUserDto appUserDto, string password);
        Task<IdentityResult> CreateModeratorAsync(AppUserDto appUserDto, string password);
        Task<bool> DeleteAsync(int id, int? adminId = null);
        Task<IList<AppUserDto>> GetUsersList();
        Task<bool> LockAsync(int id, int? days, int? adminId = null);
        Task<bool> UnLockAsync(int id, int? adminId = null);

        IQueryable<AppUser> GetItemsOnPage(int page, int pageSize, IQueryable<AppUser> baseQuery);
        Task<int> GetCountOfPlayers();
        IQueryable<AppUser> FilterByName(string name, IQueryable<AppUser> baseQuery);
        IQueryable<AppUser> SortByParam(SortState sortOrder, bool enableDeleted, bool enableLocked, IQueryable<AppUser> baseQuery);
        Task<UserIndexDto> GetPageInfo(string name, int page, int pageSize,
            SortState sortOrder, bool enableDeleted, bool enableLocked);
    }
}
