using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using WTP.BLL.ModelsDto;
using WTP.BLL.TransferModels;
using WTP.DAL.Services;
using WTP.WebApi.WTP.DAL.DomainModels;
using WTP.WebApi.WTP.DAL.Services.AppUserService;

namespace WTP.BLL.Services.AppUserDtoService
{
    public class AppUserDtoService : IAppUserDtoService
    {
        private readonly IAppUserService _appUserService;
        private readonly IMaintainableDto<LanguageDto> _maintainableLangDto;
        private readonly IMapper _mapper;

        public AppUserDtoService(IAppUserService appUserService, IMaintainableDto<LanguageDto> maintainableLangDto,
            IMapper mapper)
        {
            _appUserService = appUserService;
            _maintainableLangDto = maintainableLangDto;
            _mapper = mapper;
        }
               
        public async Task<IdentityResult> CreateAsync(AppUserDto appUserDto, string password)
        {
            var appUser = _mapper.Map<AppUser>(appUserDto);

            return await _appUserService.CreateAsync(appUser, password);
        }

        public async Task<AppUserDto> GetAsync(string id)
        {
            var appUser = await _appUserService.GetAsync(id);

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
                    Language = await _maintainableLangDto.GetAsync(item.LanguageId)
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
            var appUser = await _appUserService.GetByEmailAsync(email);

            return _mapper.Map<AppUserDto>(appUser);
        }

        public async Task<IdentityResult> UpdateAsync(AppUserDto appUserDto)
        {
            var appUser = _mapper.Map<AppUser>(appUserDto);

            return await _appUserService.UpdateAsync(appUser);
        }

        public async Task<IList<string>> GetRolesAsync(AppUserDto appUserDto)
        {
            var appUser = _mapper.Map<AppUser>(appUserDto);

            return await _appUserService.GetRolesAsync(appUser);
        }

        public async Task<bool> CheckPasswordAsync(string id, string password)
        {
            return await _appUserService.CheckPasswordAsync(id, password);
        }

        public async Task<string> GetPasswordResetTokenAsync(AppUserDto applicationUserDto)
        {
            var appUser = _mapper.Map<AppUser>(applicationUserDto);

            return await _appUserService.GetPasswordResetTokenAsync(appUser);
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MailMessage("avg0test0@gmail.com", email);
            emailMessage.Subject = subject;
            emailMessage.IsBodyHtml = true;
            emailMessage.Body = message;

            using (var client = new SmtpClient())
            {
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("avg0test0@gmail.com", "TeSt159357");
                await client.SendMailAsync(emailMessage);
            }
        }
    }
}
