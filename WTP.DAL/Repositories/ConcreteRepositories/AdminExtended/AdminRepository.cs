using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WTP.DAL.DomainModels;
using WTP.DAL.Repositories.ConcreteRepositories.AppUserExtended;
using WTP.DAL.Repositories.GenericRepository;

namespace WTP.DAL.Repositories.ConcreteRepositories.AdminExtended
{
    public class AdminRepository : RepositoryBase<Admin>, IRepository<Admin>, IAdminRepository
    {
        private readonly UserManager<Admin> _adminManager;
        private readonly ApplicationDbContext _context;

        public AdminRepository(ApplicationDbContext context, UserManager<Admin> userManager) : base(context)
        {
            _adminManager = userManager;
            _context = context;
        }

        public async Task<IdentityResult> CreateAsync(Admin appUser, string password)
        {
            var result = await _adminManager.CreateAsync(appUser, password);

            await _adminManager.AddToRoleAsync(appUser, "Admin");

            return result;
        }

        public new async Task<IdentityResult> UpdateAsync(Admin appUser)
        {
            var user = await GetAsync(appUser.Id);

            user.UserName = appUser.UserName;
            user.Email = appUser.Email;

            return await _adminManager.UpdateAsync(user);
        }

        public async Task<Admin> GetByEmailAsync(string email)
        {
            return await _adminManager.FindByEmailAsync(email);
        }

        public async Task<IList<string>> GetRolesAsync(Admin appUser)
        {
            return await _adminManager.GetRolesAsync(appUser);
        }

        public async Task<bool> CheckPasswordAsync(int id, string password)
        {
            var appUser = await _adminManager.FindByIdAsync(id.ToString());

            return await _adminManager.CheckPasswordAsync(appUser, password);
        }

        public override async Task<Admin> GetAsync(int id)
        {
            return await _context.Admins.FirstOrDefaultAsync(_ => _.Id == id);
        }
    }
}
