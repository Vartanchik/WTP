using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WTP.BLL.ModelsDto.AppUser;
using WTP.BLL.ModelsDto.AppUserLanguage;
using WTP.BLL.ModelsDto.History;
using WTP.BLL.Services.Concrete.HistoryService;
using WTP.BLL.Services.Concrete.LanguageService;
using WTP.BLL.Shared.SortState;
using WTP.DAL.DomainModels;
using WTP.DAL.Repositories.ConcreteRepositories.AppUserExtended;

namespace WTP.BLL.Services.Concrete.AppUserService
{
    public class AppUserService : IAppUserService
    {
        private readonly IMapper _mapper;
        private readonly IAppUserRepository _appUserRepository;
        private ILanguageService _languageService;
        private readonly IHistoryService _historyService;

        public AppUserService(IMapper mapper, IAppUserRepository appUserRepository, ILanguageService languageService, IHistoryService historyService)
        {
            _mapper = mapper;
            _appUserRepository = appUserRepository;
            _languageService = languageService;
            _historyService = historyService;
        }

        public async Task<IdentityResult> CreateAsync(AppUserDto appUserDto, string password, int adminId = 1)
        {
            var appUser = _mapper.Map<AppUser>(appUserDto);

            var result = await _appUserRepository.CreateAsync(appUser, password);

            HistoryDto history = new HistoryDto
            {
                DateOfOperation = DateTime.Now,
                Description = "Create new user account",
                PreviousUserEmail = null,
                PreviousUserName = null,
                NewUserEmail = appUserDto.Email,
                NewUserName = appUserDto.UserName,
                AppUserId = appUser.Id,
                OperationId = (int)OperationEnum.Create,
                AdminId = adminId
            };

            await _historyService.CreateAsync(history);

            return result;
        }

        public async Task<AppUserDto> GetAsync(int id)
        {
            var appUser = await _appUserRepository.GetAsync(id);

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

        public async Task<AppUserDto> GetByNameAsync(string name)
        {
            var appUser = await _appUserRepository.GetByNameAsync(name);

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
                     
        public async Task<IdentityResult> UpdateAsync(AppUserDto appUserDto, int adminId = 1)
        {
            var user = await _appUserRepository.GetAsync(appUserDto.Id);
            HistoryDto history = new HistoryDto
            {
                DateOfOperation = DateTime.Now,
                Description = "Update user's account",
                PreviousUserEmail = user.Email,
                PreviousUserName = user.UserName,
                NewUserEmail = appUserDto.Email,
                NewUserName = appUserDto.UserName,
                AppUserId = appUserDto.Id,
                OperationId = (int)OperationEnum.Update,
                AdminId = adminId
            };

            var appUser = _mapper.Map<AppUser>(appUserDto);
            var result = await _appUserRepository.UpdateAsync(appUser);
            
            
            await _historyService.CreateAsync(history);

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

        public async Task<string> GenerateEmailConfirmationTokenAsync(AppUserDto appUserDto)
        {
            var appUser = _mapper.Map<AppUser>(appUserDto);

            return await _appUserRepository.GenerateEmailConfirmationTokenAsync(appUser);
        }

        public async Task<AppUserDto> FindByIdAsync(string id)
        {
            var appUser = await _appUserRepository.FindByIdAsync(id);

            return _mapper.Map<AppUserDto>(appUser);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(AppUserDto appUserDto, string token)
        {
            var appUser = _mapper.Map<AppUser>(appUserDto);

            return await _appUserRepository.ConfirmEmailAsync(appUser, token);
        }

        public async Task<IdentityResult> CreateAdminAsync(AppUserDto appUserDto, string password)
        {
            var appUser = _mapper.Map<AppUser>(appUserDto);

            var result = await _appUserRepository.CreateAdminAsync(appUser, password);

            return result;

        }

        public async Task<IdentityResult> CreateModeratorAsync(AppUserDto appUserDto, string password)
        {
            var appUser = _mapper.Map<AppUser>(appUserDto);

            var result = await _appUserRepository.CreateModeratorAsync(appUser, password);

            return result;
        }

        public async Task<bool> DeleteAsync(int id, int adminId = 1)
        {
            var user = await _appUserRepository.GetAsync(id);
            HistoryDto history = new HistoryDto
            {
                DateOfOperation = DateTime.Now,
                Description = "Delete user's account",
                PreviousUserEmail = user.Email,
                PreviousUserName = user.UserName,
                NewUserEmail = user.Email,
                NewUserName = user.UserName,
                AppUserId = user.Id,
                OperationId = (int)OperationEnum.Delete,
                AdminId = adminId
            };

            await _historyService.CreateAsync(history);

            return await _appUserRepository.DeleteAsync(id);
        }

        public async Task<IList<AppUserDto>> GetAllUsersAsync()
        {
            var allUsers = await _appUserRepository.GetAllUsersAsync();
            return _mapper.Map<IList<AppUserDto>>(allUsers);
        }

        public async Task<bool> LockAsync(int id, int? days, int adminId = 1)
        {
            var user = await _appUserRepository.GetAsync(id);

            HistoryDto history = new HistoryDto
            {
                DateOfOperation = DateTime.Now,
                Description = "Lock user's account",
                PreviousUserEmail = user.Email,
                PreviousUserName = user.UserName,
                NewUserEmail = user.Email,
                NewUserName = user.UserName,
                AppUserId = user.Id,
                OperationId = (int)OperationEnum.Lock,
                AdminId = adminId
            };

            await _historyService.CreateAsync(history);

            return await _appUserRepository.LockAsync(id, days);
        }

        public async Task<bool> UnLockAsync(int id, int adminId = 1)
        {
            var user = await _appUserRepository.GetAsync(id);

            HistoryDto history = new HistoryDto
            {
                DateOfOperation = DateTime.Now,
                Description = "UnLock user's account",
                PreviousUserEmail = user.Email,
                PreviousUserName = user.UserName,
                NewUserEmail = user.Email,
                NewUserName = user.UserName,
                AppUserId = user.Id,
                OperationId = (int)OperationEnum.UnLock,
                AdminId = adminId
            };

            await _historyService.CreateAsync(history);
            return await _appUserRepository.UnLockAsync(id);
        }

        public List<AppUserDto> Filter(List<AppUserDto> users, string name)
        {
            if (users == null)
                return null;
            //return null;

            if (!String.IsNullOrEmpty(name))
            {
                users = users.Where(p => p.UserName.Contains(name)).ToList();
            }

            return users;
        }

        public List<AppUserDto> Sort(List<AppUserDto> users, SortState sortOrder, bool enableDeleted, bool enableLocked)
        {
            if (users == null)
                return null;

            switch (sortOrder)
            {
                case SortState.NameDesc:
                    users = users.OrderByDescending(s => s.UserName).ToList();
                    break;
                case SortState.EmailAsc:
                    users = users.OrderBy(s => s.Email).ToList();
                    break;
                case SortState.EmailDesc:
                    users = users.OrderByDescending(s => s.Email).ToList();
                    break;
                case SortState.IdAsc:
                    users = users.OrderBy(s => s.Id).ToList();
                    break;
                case SortState.IdDesc:
                    users = users.OrderByDescending(s => s.Id).ToList();
                    break;
                default:
                    users = users.OrderBy(s => s.UserName).ToList();
                    break;
            }

            if (!enableDeleted)
                users = users.Where(s => s.DeletedStatus == false).ToList();

            if (!enableLocked)
                users = users.Where(s => s.LockoutEnd == null).ToList();

            return users;
        }
    }
}
