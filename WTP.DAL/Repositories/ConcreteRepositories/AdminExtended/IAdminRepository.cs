using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WTP.DAL.DomainModels;

namespace WTP.DAL.Repositories.ConcreteRepositories.AdminExtended
{
    public interface IAdminRepository
    {
        Task<IdentityResult> CreateAsync(Admin appUser, string password);
        Task<IdentityResult> UpdateAsync(Admin appUser);
        Task<Admin> GetByEmailAsync(string email);
        Task<IList<string>> GetRolesAsync(Admin appUser);
        Task<bool> CheckPasswordAsync(int id, string password);
        Task<Admin> GetAsync(int id);
    }
}
