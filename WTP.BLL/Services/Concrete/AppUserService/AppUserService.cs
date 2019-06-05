using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WTP.BLL.DTOs.AppUserDTOs;
using WTP.BLL.DTOs.ServicesDTOs;
using WTP.BLL.Shared;
using WTP.DAL.Entities;
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

        public async Task<IdentityResult> CreateAsync(AppUserDto dto, string password, int? adminId = null)
        {
            var user = _mapper.Map<AppUser>(dto);

            var result = await _uow.AppUsers.CreateAsync(user, password);

            HistoryDto history = new HistoryDto
            {
                Id=0,
                DateOfOperation = DateTime.Now,
                Description = "Create new user account",
                PreviousUserEmail = null,
                PreviousUserName = null,
                NewUserEmail = dto.Email,
                NewUserName = dto.UserName,
                AppUserId = user.Id,
                OperationId = (int)OperationEnum.Create,
                AdminId = adminId
            };

            //await _uow.Histories.CreateOrUpdate(history);
            var record = _mapper.Map<History>(history);
            await _uow.Histories.CreateOrUpdate(record);
            if(result.Succeeded)
                await _uow.CommitAsync();

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
                     
        public async Task<IdentityResult> UpdateAsync(AppUserDto appUserDto, int? adminId=null)
        {
            if (await _uow.AppUsers.GetByIdAsync(appUserDto.Id) != null)
            {
                var appUser = _mapper.Map<AppUser>(appUserDto);

                HistoryDto history = new HistoryDto
                {
                    DateOfOperation = DateTime.Now,
                    Description = "Update user's account",
                    PreviousUserEmail = appUser.Email,
                    PreviousUserName = appUser.UserName,
                    NewUserEmail = appUserDto.Email,
                    NewUserName = appUserDto.UserName,
                    AppUserId = appUserDto.Id,
                    OperationId = (int)OperationEnum.Update,
                    AdminId = adminId
                };

                var record = _mapper.Map<History>(history);
                await _uow.Histories.CreateOrUpdate(record);
                await _uow.CommitAsync();

                return await _uow.AppUsers.UpdateAsync(appUser);
            }

            return IdentityResult.Failed();
        }

        public async Task<IList<string>> GetRolesAsync(AppUserDto appUserDto)
        {
            var appUser = _mapper.Map<AppUser>(appUserDto);

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

        public async Task<string> GeneratePasswordResetTokenAsync(AppUserDto appUserDto)
        {
            var appUser = _mapper.Map<AppUser>(appUserDto);

            return await _uow.AppUsers.GeneratePasswordResetTokenAsync(appUser);
        }

        public async Task<IdentityResult> ResetPasswordAsync(ResetPasswordDto dto)
        {
            var appUser = _uow.AppUsers.GetByIdAsync(dto.Id);

            return appUser == null
                ? IdentityResult.Failed()
                : await _uow.AppUsers.ResetPasswordAsync(appUser.Result,
                                                         dto.Token,
                                                         dto.NewPassword);
        }

        public async Task<IdentityResult> ChangePasswordAsync(ChangePasswordDto dto)
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

        public async Task<string> GenerateEmailConfirmationTokenAsync(AppUserDto appUserDto)
        {
            var appUser = _mapper.Map<AppUser>(appUserDto);

            return await _uow.AppUsers.GenerateEmailConfirmationTokenAsync(appUser);
        }

        public async Task<AppUserDto> FindByIdAsync(string userId)
        {
            var appUser = await _uow.AppUsers.GetByIdAsync(userId);

            return _mapper.Map<AppUserDto>(appUser);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(int userId, string token)
        {
            return _uow.AppUsers.GetByIdAsync(userId) == null || token == null
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

            if (user == null)
            {
                return string.Empty;
            }

            var token = await _uow.RestoreTokens.GetByUserId(userId);

            if (token == null)
            {
                token = new RestoreToken { AppUserId = userId, AppUser = user };
            }

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

            if (user == null)
            {
                return false;
            }

            var currentToken = await _uow.RestoreTokens.GetByUserId(userId);

            if (currentToken != null && token != currentToken.Value)
            {
                return false;
            }

            if (currentToken.ExpiryDate < DateTime.Now)
            {
                return false;
            }

            user.IsDeleted = false;

            await _uow.AppUsers.CreateOrUpdate(user);
            await _uow.RestoreTokens.DeleteAsync(currentToken.Id);
            await _uow.CommitAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id, int? adminId = null)
        {
            var user = await _uow.AppUsers.GetByIdAsync(id);
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

            var record = _mapper.Map<History>(history);
            await _uow.Histories.CreateOrUpdate(record);

            return await _uow.AppUsers.DeleteAsync(id);
        }

        public async Task<IList<AppUserDto>> GetUsersList()
        {
            var allUsers = await _uow.AppUsers.AsQueryable().Where(u=>!u.IsDeleted).ToListAsync();
            return _mapper.Map<IList<AppUserDto>>(allUsers);
        }

        public async Task<IdentityResult> CreateAdminAsync(AppUserDto appUserDto, string password)
        {
            var appUser = _mapper.Map<AppUser>(appUserDto);
            appUser.EmailConfirmed = true;

            var result = await _uow.AppUsers.CreateAdminAsync(appUser, password);

            return result;

        }

        public async Task<IdentityResult> CreateModeratorAsync(AppUserDto appUserDto, string password)
        {
            var appUser = _mapper.Map<AppUser>(appUserDto);

            var result = await _uow.AppUsers.CreateModeratorAsync(appUser, password);

            return result;
        }


        public async Task<bool> LockAsync(int id, int? days, int? adminId = null)
        {
            var user = await _uow.AppUsers.GetByIdAsync(id);

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

            var record = _mapper.Map<History>(history);
            await _uow.Histories.CreateOrUpdate(record);

            return await _uow.AppUsers.LockAsync(id, days);
        }

        public async Task<bool> UnLockAsync(int id, int? adminId = null)
        {
            var user = await _uow.AppUsers.GetByIdAsync(id);

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

            var record = _mapper.Map<History>(history);
            await _uow.Histories.CreateOrUpdate(record);

            return await _uow.AppUsers.UnLockAsync(id);
        }

        public List<AppUserDto> FilterByName(List<AppUserDto> users, string name)
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

        public List<AppUserDto> SortByParam(List<AppUserDto> users, SortState sortOrder, bool enableDeleted, bool enableLocked)
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
                users = users.Where(s => s.IsDeleted == false).ToList();

            if (!enableLocked)
                users = users.Where(s => s.LockoutEnd == null).ToList();

            return users;
        }

        public async Task<List<AppUserDto>> GetItemsOnPage(int page,int pageSize)
        {
            var items = await _uow.AppUsers.AsQueryable().Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return _mapper.Map<List<AppUserDto>>(items);
        }

        public async Task<int> GetCountOfPlayers()
        {
            return await _uow.AppUsers.AsQueryable().CountAsync();
        }

        public async Task<UserIndexDto> GetPageInfo(string name, int page, int pageSize,
            SortState sortOrder, bool enableDeleted, bool enableLocked)
        {
            //int pageSize = 3;
            var items = await this.GetItemsOnPage(page, pageSize);
            var count = await this.GetCountOfPlayers();
            items = this.FilterByName(items, name);
            items = this.SortByParam(items, sortOrder, enableDeleted, enableLocked);

            UserIndexDto viewModel = new UserIndexDto
            {
                PageViewModel = new PageDto(count, page, pageSize),
                SortViewModel = new UserSortDto(sortOrder),
                //FilterViewModel = new UserFilterViewModel(users/*(List<AppUserDto>)await _appUserService.GetAllUsersAsync()*/, name),
                Users = items
            };
            return viewModel;
        }
    }
}
