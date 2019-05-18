using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.BLL.Models.AppUser;
using WTP.DAL.Entities;
using WTP.DAL.UnitOfWork;

namespace WTP.BLL.Services.Concrete.AppUserService
{
    public class AppUserService : IAppUserService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _ouw;

        /*
        public AppUserService(IMapper mapper, Func<string, IAppUserRepository> appUserRepository)
        {
            _mapper = mapper;
            _ouw = appUserRepository("CACHE");
        }
        */

        public AppUserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _ouw = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IdentityResult> CreateAsync(AppUserModel dto, string password)
        {
            var user = _mapper.Map<AppUser>(dto);

            var result = await _ouw.AppUsers.CreateAsync(user, password);

            return result;
        }

        public async Task<AppUserModel> GetAsync(int userId)
        {
            var appUser = await _ouw.AppUsers.GetAsync(userId);

            var appUserDto = _mapper.Map<AppUserModel>(appUser);

            return appUserDto;
        }

        public async Task<AppUserModel> GetByEmailAsync(string email)
        {
            var appUser = await _ouw.AppUsers.GetByEmailAsync(email);

            var appUserDto = _mapper.Map<AppUserModel>(appUser);

            return appUserDto;
        }

        public async Task<AppUserModel> GetByNameAsync(string userName)
        {
            var appUser = await _ouw.AppUsers.GetByNameAsync(userName);

            var appUserDto = _mapper.Map<AppUserModel>(appUser);

            return appUserDto;
        }
                     
        public async Task<IdentityResult> UpdateAsync(AppUserModel appUserDto)
        {
            if (await _ouw.AppUsers.GetAsync(appUserDto.Id) != null)
            {
                var appUser = _mapper.Map<AppUser>(appUserDto);

                return await _ouw.AppUsers.UpdateAsync(appUser);
            }

            return IdentityResult.Failed();
        }

        public async Task<IList<string>> GetRolesAsync(AppUserModel appUserDto)
        {
            var appUser = _mapper.Map<AppUser>(appUserDto);

            return await _ouw.AppUsers.GetRolesAsync(appUser);
        }

        public async Task<bool> CheckPasswordAsync(int userId, string password)
        {
            return await _ouw.AppUsers.CheckPasswordAsync(userId, password);
        }

        public async Task<bool> IsEmailConfirmedAsync(int userId)
        {
            return await _ouw.AppUsers.IsEmailConfirmedAsync(userId);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(AppUserModel appUserDto)
        {
            var appUser = _mapper.Map<AppUser>(appUserDto);

            return await _ouw.AppUsers.GeneratePasswordResetTokenAsync(appUser);
        }

        public async Task<IdentityResult> ResetPasswordAsync(ResetPasswordModel resetPasswordDto)
        {
            var appUser = _ouw.AppUsers.GetAsync(resetPasswordDto.Id);

            return appUser == null
                ? IdentityResult.Failed()
                : await _ouw.AppUsers.ResetPasswordAsync(appUser.Result,
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

            return await _ouw.AppUsers.ChangePasswordAsync(changePasswordDto.UserId,
                                                                changePasswordDto.CurrentPassword,
                                                                changePasswordDto.NewPassword);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(AppUserModel appUserDto)
        {
            var appUser = _mapper.Map<AppUser>(appUserDto);

            return await _ouw.AppUsers.GenerateEmailConfirmationTokenAsync(appUser);
        }

        public async Task<AppUserModel> FindByIdAsync(string userId)
        {
            var appUser = await _ouw.AppUsers.FindByIdAsync(userId);

            return _mapper.Map<AppUserModel>(appUser);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(int userId, string token)
        {
            return _ouw.AppUsers.GetAsync(userId) == null || token == null
                ? IdentityResult.Failed() 
                : await _ouw.AppUsers.ConfirmEmailAsync(userId, token);
        }
    }
}
