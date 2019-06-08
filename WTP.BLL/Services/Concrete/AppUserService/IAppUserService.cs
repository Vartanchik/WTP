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
        Task<IdentityResult> CreateAsync(AppUserDto dto, string password, int? adminId = null);
        Task<AppUserDto> GetByIdAsync(int userId);
        Task<AppUserDto> GetByEmailAsync(string email);
        Task<AppUserDto> GetByNameAsync(string userName);
        Task<IdentityResult> UpdateAsync(AppUserDto dto, int? adminId = null);
        Task<IList<string>> GetRolesAsync(AppUserDto dto);
        Task<bool> CheckPasswordAsync(int userId, string password);
        Task<bool> IsEmailConfirmedAsync(int userId);
        Task<string> GeneratePasswordResetTokenAsync(AppUserDto dto);
        Task<IdentityResult> ResetPasswordAsync(ResetPasswordDto dto);
        Task<IdentityResult> ChangePasswordAsync(ChangeUserPasswordDto dto);
        Task<string> GenerateEmailConfirmationTokenAsync(AppUserDto dto);
        Task<AppUserDto> FindByIdAsync(string userId);
        Task<IdentityResult> ConfirmEmailAsync(int userId, string token);
        Task DeleteAccountAsync(int userId, int? adminId = null);
        Task<string> CreateRestoreAccountToken(int userId);
        Task<bool> RestoreAccountAsync(int userId, string token);
        Task<UserIconDto> GetUserIconAsync(int userId);

        //For Admin
        Task<IdentityResult> CreateAdminAsync(AppUserDto appUserDto, string password);
        Task<IdentityResult> CreateModeratorAsync(AppUserDto appUserDto, string password);
        Task<IList<AppUserDto>> GetUsersList();
        Task<bool> LockAsync(int id, int? days, int? adminId = null);
        Task<bool> UnLockAsync(int id, int? adminId = null);

        Task<IQueryable<AppUser>> GetItemsOnPage(int page, int pageSize, IQueryable<AppUser> baseQuery);
        Task<int> GetCountOfPlayers();
        Task<IQueryable<AppUser>> FilterByName(string name, IQueryable<AppUser> baseQuery);
        Task<IQueryable<AppUser>> SortByParam(SortState sortOrder, bool enableDeleted, bool enableLocked, IQueryable<AppUser> baseQuery);
        Task<UserIndexDto> GetPageInfo(string name, int page, int pageSize,
            SortState sortOrder, bool enableDeleted, bool enableLocked);
    }
}
