using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WTP.BLL.ModelsDto.Admin;

namespace WTP.BLL.Services.Concrete.AdminService
{
    public interface IAdminService
    {
        Task<IdentityResult> CreateAsync(AdminDto adminDto, string password);
        Task<AdminDto> GetAsync(int id);
        Task<AdminDto> GetByEmailAsync(string email);
        Task<IdentityResult> UpdateAsync(AdminDto adminDto);
        Task<IList<string>> GetRolesAsync(AdminDto adminDto);
        Task<bool> CheckPasswordAsync(int id, string password);
    }
}
