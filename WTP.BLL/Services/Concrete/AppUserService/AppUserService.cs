using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WTP.BLL.DTOs.AppUserDTOs;
using WTP.BLL.DTOs.ServicesDTOs;
using WTP.DAL.Entities.AppUserEntities;
using WTP.DAL.UnitOfWork;

namespace WTP.BLL.Services.Concrete.AppUserService
{
    public class AppUserService : IAppUserService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;


        public AppUserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _uow = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IdentityResult> CreateAsync(AppUserDto dto, string password)
        {
            var user = _mapper.Map<AppUser>(dto);

            var result = await _uow.AppUsers.CreateAsync(user, password);

            return result;
        }

        public async Task<AppUserDto> GetByIdAsync(int userId)
        {
            var appUser = await _uow.AppUsers.GetByIdAsync(userId);

            var appUserDto = _mapper.Map<AppUserDto>(appUser);

            return appUserDto;
        }

        public async Task<AppUserDto> GetByEmailAsync(string email)
        {
            var appUser = await _uow.AppUsers.GetByEmailAsync(email);

            var appUserDto = _mapper.Map<AppUserDto>(appUser);

            return appUserDto;
        }

        public async Task<AppUserDto> GetByNameAsync(string userName)
        {
            var appUser = await _uow.AppUsers.GetByNameAsync(userName);

            var appUserDto = _mapper.Map<AppUserDto>(appUser);

            return appUserDto;
        }
                     
        public async Task<IdentityResult> UpdateAsync(AppUserDto dto)
        {
            if (await _uow.AppUsers.AsQueryable().AnyAsync(u => u.Id == dto.Id))
            {
                var appUser = _mapper.Map<AppUser>(dto);

                return await _uow.AppUsers.UpdateAsync(appUser);
            }

            return IdentityResult.Failed();
        }

        public async Task<IList<string>> GetRolesAsync(AppUserDto dto)
        {
            var appUser = _mapper.Map<AppUser>(dto);

            return await _uow.AppUsers.GetRolesAsync(appUser);
        }

        public async Task<bool> CheckPasswordAsync(int userId, string password)
        {
            return await _uow.AppUsers.CheckPasswordAsync(userId, password);
        }

        public async Task<bool> IsEmailConfirmedAsync(int userId)
        {
            return await _uow.AppUsers.IsEmailConfirmedAsync(userId);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(AppUserDto dto)
        {
            var appUser = _mapper.Map<AppUser>(dto);

            return await _uow.AppUsers.GeneratePasswordResetTokenAsync(appUser);
        }

        public async Task<IdentityResult> ResetPasswordAsync(ResetPasswordDto dto)
        {
            var appUser = await _uow.AppUsers.GetByIdAsync(dto.Id);

            return appUser == null
                ? IdentityResult.Failed()
                : await _uow.AppUsers.ResetPasswordAsync(appUser,
                                                         dto.Token,
                                                         dto.NewPassword);
        }

        public async Task<IdentityResult> ChangePasswordAsync(ChangeUserPasswordDto dto)
        {
            if (await GetByIdAsync(dto.UserId) == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Something going wrong." });
            }

            if (!await CheckPasswordAsync(dto.UserId, dto.CurrentPassword))
            {
                return IdentityResult.Failed(new IdentityError { Description = "Invalid current password." });
            }

            return await _uow.AppUsers.ChangePasswordAsync(dto.UserId,
                                                           dto.CurrentPassword,
                                                           dto.NewPassword);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(AppUserDto dto)
        {
            var appUser = _mapper.Map<AppUser>(dto);

            return await _uow.AppUsers.GenerateEmailConfirmationTokenAsync(appUser);
        }

        public async Task<AppUserDto> FindByIdAsync(string userId)
        {
            var appUser = await _uow.AppUsers.GetByIdAsync(userId);

            return _mapper.Map<AppUserDto>(appUser);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(int userId, string token)
        {
            var existed = await _uow.AppUsers.AsQueryable().AnyAsync(u => u.Id == userId);

            return !existed || token == null
                ? IdentityResult.Failed() 
                : await _uow.AppUsers.ConfirmEmailAsync(userId, token);
        }

        public async Task DeleteAccountAsync(int userId)
        {
            var user = await _uow.AppUsers.GetByIdAsync(userId);

            if (user != null)
            {
                user.IsDeleted = true;
                user.DeletedTime = DateTime.Today;

                await _uow.AppUsers.CreateOrUpdate(user);
                await _uow.CommitAsync();
            }
        }

        public async Task<string> CreateRestoreAccountToken(int userId)
        {
            var user = await _uow.AppUsers.GetByIdAsync(userId);

            if (user == null) return string.Empty;

            var token = await _uow.RestoreTokens.GetByUserId(userId);

            if (token == null) token = new RestoreToken { AppUserId = userId, AppUser = user };

            token.Value = Guid.NewGuid().ToString();
            token.CreateDate = DateTime.Now;
            token.ExpiryDate = DateTime.Now.AddDays(1);

            await _uow.RestoreTokens.CreateOrUpdate(token);
            await _uow.CommitAsync();

            return token.Value;
        }

        public async Task<bool> RestoreAccountAsync(int userId, string token)
        {
            var user = await _uow.AppUsers.GetByIdAsync(userId);

            if (user == null) return false;

            var currentToken = await _uow.RestoreTokens.GetByUserId(userId);

            if (currentToken != null && token != currentToken.Value) return false;

            if (currentToken.ExpiryDate < DateTime.Now) return false;

            user.IsDeleted = false;

            await _uow.AppUsers.CreateOrUpdate(user);
            await _uow.RestoreTokens.DeleteAsync(currentToken.Id);
            await _uow.CommitAsync();

            return true;
        }

        public async Task<UserIconDto> GetUserIconAsync(int userId)
        {
            return await _uow.AppUsers.AsQueryable()
                                .Where(u => u.Id == userId)
                                .Select(x => new UserIconDto { UserName = x.UserName, Photo = x.Photo })
                                .FirstOrDefaultAsync();
        }
    }
}
