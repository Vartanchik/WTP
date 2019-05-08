using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.BLL.ModelsDto.AppUser;
using WTP.BLL.ModelsDto.AppUserLanguage;
using WTP.BLL.Services.Concrete.LanguageService;
using WTP.DAL.DomainModels;
using WTP.DAL.Repositories.ConcreteRepositories.AppUserExtended;

namespace WTP.BLL.Services.Concrete.AppUserService
{
    public class AppUserService : IAppUserService
    {
        private readonly IMapper _mapper;
        private readonly IAppUserRepository _appUserRepository;
        private ILanguageService _languageService;
        private IConfiguration _configuration;

        public AppUserService(IMapper mapper, Func<string, IAppUserRepository> appUserRepository, ILanguageService languageService, IConfiguration configuration)
        {
            _mapper = mapper;
            _appUserRepository = appUserRepository("CACHE");
            _languageService = languageService;
            _configuration = configuration;
        }

        public async Task<IdentityResult> CreateAsync(AppUserDto appUserDto, string password)
        {
            var appUser = _mapper.Map<AppUser>(appUserDto);

            var result = await _appUserRepository.CreateAsync(appUser, password);

            return result;
        }

        public async Task<AppUserDto> GetAsync(int userId)
        {
            var appUser = await _appUserRepository.GetAsync(userId);

            var appUserDto = _mapper.Map<AppUserDto>(appUser);

            if (appUser != null)
            {
                // Create new List in order to get current language
                var appUserDtoLanguagesDto = new List<AppUserDtoLanguageDto>();

                foreach (var item in appUserDto.AppUserLanguages)
                {
                    appUserDtoLanguagesDto.Add(new AppUserDtoLanguageDto
                    {
                        AppUser = null,
                        AppUserId = null,
                        LanguageId = item.LanguageId,
                        Language = await _languageService.GetAsync(item.LanguageId)
                    });
                }

                //Set language property 
                appUserDto.AppUserLanguages = appUserDtoLanguagesDto;

                //Set default user photo
                if (string.IsNullOrEmpty(appUserDto.Photo))
                {
                    appUserDto.Photo = _configuration["Photo:DefaultPhoto"];
                }

                return appUserDto;
            }

            return appUserDto;
        }

        public async Task<AppUserDto> GetByEmailAsync(string email)
        {
            var appUser = await _appUserRepository.GetByEmailAsync(email);

            var appUserDto = _mapper.Map<AppUserDto>(appUser);

            if (appUser != null)
            {
                // Create new List in order to get current language
                var appUserDtoLanguagesDto = new List<AppUserDtoLanguageDto>();

                foreach (var item in appUserDto.AppUserLanguages)
                {
                    appUserDtoLanguagesDto.Add(new AppUserDtoLanguageDto
                    {
                        AppUser = null,
                        AppUserId = null,
                        LanguageId = item.LanguageId,
                        Language = await _languageService.GetAsync(item.LanguageId)
                    });
                }

                //Set language property 
                appUserDto.AppUserLanguages = appUserDtoLanguagesDto;

                //Set default user photo
                if (appUserDto.Photo == null)
                {
                    appUserDto.Photo = "https://cdn4.iconfinder.com/data/icons/48-bubbles/48/30.User-256.png";
                }

                return appUserDto;
            }

            return appUserDto;
        }

        public async Task<AppUserDto> GetByNameAsync(string userName)
        {
            var appUser = await _appUserRepository.GetByNameAsync(userName);

            var appUserDto = _mapper.Map<AppUserDto>(appUser);

            if (appUser != null)
            {
                // Create new List in order to get current language
                var appUserDtoLanguagesDto = new List<AppUserDtoLanguageDto>();

                foreach (var item in appUserDto.AppUserLanguages)
                {
                    appUserDtoLanguagesDto.Add(new AppUserDtoLanguageDto
                    {
                        AppUser = null,
                        AppUserId = null,
                        LanguageId = item.LanguageId,
                        Language = await _languageService.GetAsync(item.LanguageId)
                    });
                }

                //Set language property 
                appUserDto.AppUserLanguages = appUserDtoLanguagesDto;

                //Set default user photo
                if (appUserDto.Photo == null)
                {
                    appUserDto.Photo = "https://cdn4.iconfinder.com/data/icons/48-bubbles/48/30.User-256.png";
                }

                return appUserDto;
            }

            return appUserDto;
        }
                     
        public async Task<IdentityResult> UpdateAsync(AppUserDto appUserDto)
        {
            appUserDto.Photo = appUserDto.Photo == _configuration["Photo:DefaultPhoto"]
                ? null
                : appUserDto.Photo;

            var appUser = _mapper.Map<AppUser>(appUserDto);

            var result = await _appUserRepository.UpdateAsync(appUser);

            return result;
        }

        public async Task<IList<string>> GetRolesAsync(AppUserDto appUserDto)
        {
            var appUser = _mapper.Map<AppUser>(appUserDto);

            return await _appUserRepository.GetRolesAsync(appUser);
        }

        public async Task<bool> CheckPasswordAsync(int userId, string password)
        {
            return await _appUserRepository.CheckPasswordAsync(userId, password);
        }

        public async Task<bool> IsEmailConfirmedAsync(AppUserDto appUserDto)
        {
            var appUser = _mapper.Map<AppUser>(appUserDto);

            return await _appUserRepository.IsEmailConfirmedAsync(appUser);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(AppUserDto appUserDto)
        {
            var appUser = _mapper.Map<AppUser>(appUserDto);

            return await _appUserRepository.GeneratePasswordResetTokenAsync(appUser);
        }

        public async Task<IdentityResult> ResetPasswordAsync(AppUserDto appUserDto, string token, string newPassword)
        {
            var appUser = _mapper.Map<AppUser>(appUserDto);

            return await _appUserRepository.ResetPasswordAsync(appUser, token, newPassword);
        }

        public async Task<IdentityResult> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
        {
            return await _appUserRepository.ChangePasswordAsync(changePasswordDto.UserId,
                                                                changePasswordDto.CurrentPassword,
                                                                changePasswordDto.NewPassword);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(AppUserDto appUserDto)
        {
            var appUser = _mapper.Map<AppUser>(appUserDto);

            return await _appUserRepository.GenerateEmailConfirmationTokenAsync(appUser);
        }

        public async Task<AppUserDto> FindByIdAsync(string userId)
        {
            var appUser = await _appUserRepository.FindByIdAsync(userId);

            return _mapper.Map<AppUserDto>(appUser);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(AppUserDto appUserDto, string token)
        {
            var appUser = _mapper.Map<AppUser>(appUserDto);

            return await _appUserRepository.ConfirmEmailAsync(appUser, token);
        }
    }
}
