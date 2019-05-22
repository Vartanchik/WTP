﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
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

        /*
        public AppUserService(IMapper mapper, Func<string, IAppUserRepository> appUserRepository)
        {
            _mapper = mapper;
            _uow = appUserRepository("CACHE");
        }
        */

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

        public async Task<AppUserDto> GetAsync(int userId)
        {
            var appUser = await _uow.AppUsers.GetAsync(userId);

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
                     
        public async Task<IdentityResult> UpdateAsync(AppUserDto appUserDto)
        {
            if (await _uow.AppUsers.GetAsync(appUserDto.Id) != null)
            {
                var appUser = _mapper.Map<AppUser>(appUserDto);

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
            var appUser = _uow.AppUsers.GetAsync(dto.Id);

            return appUser == null
                ? IdentityResult.Failed()
                : await _uow.AppUsers.ResetPasswordAsync(appUser.Result,
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
            var appUser = await _uow.AppUsers.FindByIdAsync(userId);

            return _mapper.Map<AppUserDto>(appUser);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(int userId, string token)
        {
            return _uow.AppUsers.GetAsync(userId) == null || token == null
                ? IdentityResult.Failed() 
                : await _uow.AppUsers.ConfirmEmailAsync(userId, token);
        }

        public async Task DeleteAsync(int userId)
        {
            var user = await _uow.AppUsers.GetAsync(userId);

            user.IsDeleted = true;
            user.DeletedTime = DateTime.Today;

            await _uow.Tokens.DeleteUserTokensAsync(user.Id);
            await _uow.AppUsers.CreateOrUpdate(user);
        }

        public async Task<bool> DeleteAsync(int id, int adminId = 1)
        {
            var user = await _uow.AppUsers.GetAsync(id);
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

        public async Task<IList<AppUserDto>> GetAllUsersAsync()
        {
            var allUsers = await _uow.AppUsers.GetAllUsersAsync();
            return _mapper.Map<IList<AppUserDto>>(allUsers);
        }

        public async Task<bool> LockAsync(int id, int? days, int adminId = 1)
        {
            var user = await _uow.AppUsers.GetAsync(id);

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

        public async Task<bool> UnLockAsync(int id, int adminId = 1)
        {
            var user = await _uow.AppUsers.GetAsync(id);

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
                users = users.Where(s => s.IsDeleted == false).ToList();

            if (!enableLocked)
                users = users.Where(s => s.LockoutEnd == null).ToList();

            return users;
        }
    }
}
