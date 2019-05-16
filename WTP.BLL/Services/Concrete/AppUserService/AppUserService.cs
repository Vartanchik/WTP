using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.BLL.Models.AppUser;
using WTP.DAL.Entities;
using WTP.DAL.Repositories.ConcreteRepositories.AppUserExtended;

namespace WTP.BLL.Services.Concrete.AppUserService
{
    public class AppUserService : IAppUserService
    {
        private readonly IMapper _mapper;
        private readonly IAppUserRepository _appUserRepository;

        public AppUserService(IMapper mapper, Func<string, IAppUserRepository> appUserRepository)
        {
            _mapper = mapper;
            _appUserRepository = appUserRepository("CACHE");
        }

        public async Task<IdentityResult> CreateAsync(AppUserModel appUserDto, string password)
        {
            var appUser = _mapper.Map<AppUser>(appUserDto);

            var result = await _appUserRepository.CreateAsync(appUser, password);

            return result;
        }

        public async Task<AppUserModel> GetAsync(int userId)
        {
            var appUser = await _appUserRepository.GetAsync(userId);

            var appUserDto = _mapper.Map<AppUserModel>(appUser);

            return appUserDto;
        }

        public async Task<AppUserModel> GetByEmailAsync(string email)
        {
            var appUser = await _appUserRepository.GetByEmailAsync(email);

            var appUserDto = _mapper.Map<AppUserModel>(appUser);

            return appUserDto;
        }

        public async Task<AppUserModel> GetByNameAsync(string userName)
        {
            var appUser = await _appUserRepository.GetByNameAsync(userName);

            var appUserDto = _mapper.Map<AppUserModel>(appUser);

            return appUserDto;
        }
                     
        public async Task<IdentityResult> UpdateAsync(AppUserModel appUserDto)
        {
            if (await _appUserRepository.GetAsync(appUserDto.Id) != null)
            {
                var appUser = _mapper.Map<AppUser>(appUserDto);

                return await _appUserRepository.UpdateAsync(appUser);
            }

            return IdentityResult.Failed();
        }

        public async Task<IList<string>> GetRolesAsync(AppUserModel appUserDto)
        {
            var appUser = _mapper.Map<AppUser>(appUserDto);

            return await _appUserRepository.GetRolesAsync(appUser);
        }

        public async Task<bool> CheckPasswordAsync(int userId, string password)
        {
            return await _appUserRepository.CheckPasswordAsync(userId, password);
        }

        public async Task<bool> IsEmailConfirmedAsync(int userId)
        {
            return await _appUserRepository.IsEmailConfirmedAsync(userId);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(AppUserModel appUserDto)
        {
            var appUser = _mapper.Map<AppUser>(appUserDto);

            return await _appUserRepository.GeneratePasswordResetTokenAsync(appUser);
        }

        public async Task<IdentityResult> ResetPasswordAsync(ResetPasswordModel resetPasswordDto)
        {
            var appUser = _appUserRepository.GetAsync(resetPasswordDto.Id);

            return appUser == null
                ? IdentityResult.Failed()
                : await _appUserRepository.ResetPasswordAsync(appUser.Result,
                                                              resetPasswordDto.Token,
                                                              resetPasswordDto.NewPassword);
        }

        public async Task<IdentityResult> ChangePasswordAsync(ChangePasswordModel changePasswordDto)
        {
            if (await GetAsync(changePasswordDto.UserId) == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Something going wrong." });
            }

            if (!await CheckPasswordAsync(changePasswordDto.UserId, changePasswordDto.CurrentPassword))
            {
                return IdentityResult.Failed(new IdentityError { Description = "Invalid current password." });
            }

            return await _appUserRepository.ChangePasswordAsync(changePasswordDto.UserId,
                                                                changePasswordDto.CurrentPassword,
                                                                changePasswordDto.NewPassword);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(AppUserModel appUserDto)
        {
            var appUser = _mapper.Map<AppUser>(appUserDto);

            return await _appUserRepository.GenerateEmailConfirmationTokenAsync(appUser);
        }

        public async Task<AppUserModel> FindByIdAsync(string userId)
        {
            var appUser = await _appUserRepository.FindByIdAsync(userId);

            return _mapper.Map<AppUserModel>(appUser);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(int userId, string token)
        {
            return _appUserRepository.GetAsync(userId) == null || token == null
                ? IdentityResult.Failed() 
                : await _appUserRepository.ConfirmEmailAsync(userId, token);
        }

        public async Task<IdentityResult> UpdatePhotoAsync(int userId, string userPhoto)
        {

            var appUserDto = await _appUserRepository.GetAsync(userId);

            appUserDto.Photo = userPhoto;

            var appUser = _mapper.Map<AppUser>(appUserDto);

            return await _appUserRepository.UpdateAsync(appUserDto);
        }
    }
}
