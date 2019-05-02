using AutoMapper;
using Microsoft.AspNetCore.Identity;
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

        public AppUserService(IMapper mapper, IAppUserRepository appUserRepository, ILanguageService languageService)
        {
            _mapper = mapper;
            _appUserRepository = appUserRepository;
            _languageService = languageService;
        }

        public async Task<IdentityResult> CreateAsync(AppUserDto appUserDto, string password)
        {
            var appUser = _mapper.Map<AppUser>(appUserDto);

            var result = await _appUserRepository.CreateAsync(appUser, password);

            return result;
        }

        public async Task<AppUserDto> GetAsync(int id)
        {
            var appUser = await _appUserRepository.GetAsync(id);

            var appUserDto = _mapper.Map<AppUserDto>(appUser);

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

        public async Task<AppUserDto> GetByEmailAsync(string email)
        {
            var appUser = await _appUserRepository.GetByEmailAsync(email);

            return _mapper.Map<AppUserDto>(appUser);
        }

        public async Task<IdentityResult> UpdateAsync(AppUserDto appUserDto)
        {
            var appUser = _mapper.Map<AppUser>(appUserDto);

            var result = await _appUserRepository.UpdateAsync(appUser);

            return result;
        }

        public async Task<IList<string>> GetRolesAsync(AppUserDto appUserDto)
        {
            var appUser = _mapper.Map<AppUser>(appUserDto);

            return await _appUserRepository.GetRolesAsync(appUser);
        }

        public async Task<bool> CheckPasswordAsync(int id, string password)
        {
            return await _appUserRepository.CheckPasswordAsync(id, password);
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
    }
}
