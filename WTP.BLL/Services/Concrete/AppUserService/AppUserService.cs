using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.BLL.DTOs.AppUserDTOs;
using WTP.BLL.DTOs.ServicesDTOs;
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

        public async Task<IdentityResult> CreateAsync(AppUserDto dto, string password)
        {
            var user = _mapper.Map<AppUser>(dto);

            var result = await _ouw.AppUsers.CreateAsync(user, password);

            return result;
        }

        public async Task<AppUserDto> GetAsync(int userId)
        {
            var appUser = await _ouw.AppUsers.GetAsync(userId);

            var appUserDto = _mapper.Map<AppUserDto>(appUser);

            return appUserDto;
        }

        public async Task<AppUserDto> GetByEmailAsync(string email)
        {
            var appUser = await _ouw.AppUsers.GetByEmailAsync(email);

            var appUserDto = _mapper.Map<AppUserDto>(appUser);

            return appUserDto;
        }

        public async Task<AppUserDto> GetByNameAsync(string userName)
        {
            var appUser = await _ouw.AppUsers.GetByNameAsync(userName);

            var appUserDto = _mapper.Map<AppUserDto>(appUser);

            return appUserDto;
        }
                     
        public async Task<IdentityResult> UpdateAsync(AppUserDto appUserDto)
        {
            if (await _ouw.AppUsers.GetAsync(appUserDto.Id) != null)
            {
                var appUser = _mapper.Map<AppUser>(appUserDto);

                return await _ouw.AppUsers.UpdateAsync(appUser);
            }

            return IdentityResult.Failed();
        }

        public async Task<IList<string>> GetRolesAsync(AppUserDto appUserDto)
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

        public async Task<string> GeneratePasswordResetTokenAsync(AppUserDto appUserDto)
        {
            var appUser = _mapper.Map<AppUser>(appUserDto);

            return await _ouw.AppUsers.GeneratePasswordResetTokenAsync(appUser);
        }

        public async Task<IdentityResult> ResetPasswordAsync(ResetPasswordDto dto)
        {
            var appUser = _ouw.AppUsers.GetAsync(dto.Id);

            return appUser == null
                ? IdentityResult.Failed()
                : await _ouw.AppUsers.ResetPasswordAsync(appUser.Result,
                                                              dto.Token,
                                                              dto.NewPassword);
        }

        public async Task<IdentityResult> ChangePasswordAsync(ChangePasswordDto dto)
        {
            if (await GetAsync(dto.UserId) == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Something going wrong." });
            }

            if (!await CheckPasswordAsync(dto.UserId, dto.CurrentPassword))
            {
                return IdentityResult.Failed(new IdentityError { Description = "Invalid current password." });
            }

            return await _ouw.AppUsers.ChangePasswordAsync(dto.UserId,
                                                                dto.CurrentPassword,
                                                                dto.NewPassword);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(AppUserDto appUserDto)
        {
            var appUser = _mapper.Map<AppUser>(appUserDto);

            return await _ouw.AppUsers.GenerateEmailConfirmationTokenAsync(appUser);
        }

        public async Task<AppUserDto> FindByIdAsync(string userId)
        {
            var appUser = await _ouw.AppUsers.FindByIdAsync(userId);

            return _mapper.Map<AppUserDto>(appUser);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(int userId, string token)
        {
            return _ouw.AppUsers.GetAsync(userId) == null || token == null
                ? IdentityResult.Failed() 
                : await _ouw.AppUsers.ConfirmEmailAsync(userId, token);
        }

        public async Task DeleteAsync(int userId)
        {
            var user = await _ouw.AppUsers.GetAsync(userId);

            user.Deleted = true;
            user.DeletedTime = DateTime.Today;

            await _ouw.Tokens.DeleteUserTokensAsync(user.Id);
            await _ouw.AppUsers.CreateOrUpdate(user);
        }
    }
}
