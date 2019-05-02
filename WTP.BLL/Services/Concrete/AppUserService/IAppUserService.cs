using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.BLL.ModelsDto.AppUser;

namespace WTP.BLL.Services.Concrete.AppUserService
{
    public interface IAppUserService
    {
        Task<IdentityResult> CreateAdminAsync(AppUserDto appUser, string password);
        Task<IdentityResult> CreateAsync(AppUserDto appUserDto, string password);
        Task<AppUserDto> GetAsync(int id);
        Task<bool> DeleteAsync(int id);
        Task<AppUserDto> GetByEmailAsync(string email);
        Task<IdentityResult> UpdateAsync(AppUserDto appUserDto);
        Task<IList<string>> GetRolesAsync(AppUserDto appUserDto);
        Task<bool> CheckPasswordAsync(int id, string password);
        Task<IEnumerable<AppUserDto>> GetAllAsync();
        Task<bool> LockAsync(int id,int? days);
        Task<bool> UnLockAsync(int id);
    }
}
