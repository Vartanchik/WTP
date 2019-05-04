﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WTP.DAL.DomainModels;

namespace WTP.DAL.Repositories.ConcreteRepositories.AppUserExtended
{
    public interface IAppUserRepository
    {
        Task<IdentityResult> CreateAsync(AppUser appUser, string password);
        Task<IdentityResult> UpdateAsync(AppUser appUser);
        Task<AppUser> GetByEmailAsync(string email);
        Task<AppUser> GetByNameAsync(string name);
        Task<IList<string>> GetRolesAsync(AppUser appUser);
        Task<bool> CheckPasswordAsync(int id, string password);
        Task<AppUser> GetAsync(int id);
        Task<bool> IsEmailConfirmedAsync(AppUser appUser);
        Task<string> GeneratePasswordResetTokenAsync(AppUser appUser);
        Task<IdentityResult> ResetPasswordAsync(AppUser appUser, string token, string newPassword);
        Task<IdentityResult> ChangePasswordAsync(int appUserId, string currentPassword, string newPassword);
        Task<string> GenerateEmailConfirmationTokenAsync(AppUser appUser);
        Task<AppUser> FindByIdAsync(string id);
        Task<IdentityResult> ConfirmEmailAsync(AppUser appUser, string token);
        Task<IdentityResult> CreateAdminAsync(AppUser appUser, string password);
        Task<IdentityResult> CreateModeratorAsync(AppUser appUser, string password);
        Task<bool> DeleteAsync(int id);
        Task<IList<AppUser>> GetAllUsersAsync();
        Task<bool> LockAsync(int id, int? days);
        Task<bool> UnLockAsync(int id);
    }
}
