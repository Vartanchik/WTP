using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.BLL.Models.AppUserModels;
using WTP.BLL.UnitOfWork;

namespace WTP.BLL.Services.Concrete.AppUserService
{
    public class AppUserService : IAppUserService
    {
        private readonly IUnitOfWork _ouw;

        public AppUserService(IUnitOfWork unitOfWork)
        {
            _ouw = unitOfWork;
        }

        public async Task<OperationResult> ChangePasswordAsync(ChangePasswordModel model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CheckPasswordAsync(int userId, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<OperationResult> ConfirmEmailAsync(int userId, string token)
        {
            throw new NotImplementedException();
        }

        public async Task<OperationResult> CreateAsync(AppUserModel model, string password)
        {
            OperationResult result;
            try
            {
                await _ouw.AppUserModels.CreateAsync(new AppUserModel { PasswordHash = password });
                /* _hasher.Hash(password) */

                result = OperationResult.Success;
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public async Task<OperationResult> DeleteAsync(int userId)
        {
            OperationResult result;
            try
            {
                await _ouw.AppUserModels.DeleteAsync(userId);
                result = OperationResult.Success;
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public async Task<AppUserModel> FindByIdAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<AppUserModel> FindByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(AppUserModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GeneratePasswordResetTokenAsync(AppUserModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<AppUserModel> GetByEmailAsync(string userEmail)
        {
            throw new NotImplementedException();
        }

        public async Task<AppUserModel> GetByNameAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<string>> GetRolesAsync(AppUserModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsEmailConfirmedAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<OperationResult> ResetPasswordAsync(ResetPasswordModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<OperationResult> UpdateAsync(AppUserModel model)
        {
            throw new NotImplementedException();
        }

        //

        /*
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
        */
    }
}
