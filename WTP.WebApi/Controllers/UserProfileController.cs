using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WTP.BLL.ModelsDto.AppUserLanguage;
using WTP.BLL.ModelsDto.Language;
using WTP.BLL.Services.Concrete.AppUserService;
using WTP.BLL.Services.Concrete.AzureBlobStorageService;
using WTP.WebAPI.Utility.Extensions;

namespace WTP.WebAPI.ViewModels.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IAppUserService _appUserService;

        private readonly IConfiguration _configuration;

        private readonly IAzureBlobStorageService _azureBlobStorageService;

        public UserProfileController(IAppUserService appUserService, IConfiguration configuration, IAzureBlobStorageService azureBlobStorageService)
        {
            _appUserService = appUserService;

            _configuration = configuration;

            _azureBlobStorageService = azureBlobStorageService;
        }

        //GET : /api/UserProfile
        [HttpGet]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> GetUserProfile()
        {
            int userId = this.GetCurrentUserId();

            var user = await _appUserService.GetAsync(userId);

            if (user == null)
            {
                return NotFound(new ResponseViewModel(404, "Something going wrong."));
            }

            var languages = new List<LanguageDto>();
            foreach (var item in user.AppUserLanguages)
            {
                languages.Add(new LanguageDto
                {
                    Id = item.LanguageId,
                    Name = item.Language.Name
                });
            }

            var appUserDtoViewModel = new AppUserDtoViewModel()
            {
                UserName = user.UserName,
                Email = user.Email,
                Photo = await _azureBlobStorageService.DownloadFileAsync(user.Photo),
                Gender = user.Gender,
                DateOfBirth = user.DateOfBirth.ToString(),
                Country = user.Country,
                Steam = user.Steam,
                Languages = languages,
                Players = user.Players,
                Teams = user.Teams
            };

            return Ok(appUserDtoViewModel);
        }

        //PUT : /api/UserProfile
        [HttpPut]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> UpdateUserProfile([FromBody]AppUserDtoViewModel formdata)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseViewModel(400, "Invalid value was entered! Please, redisplay form."));
            }

            int userId = this.GetCurrentUserId();

            var user = await _appUserService.GetAsync(userId);

            if (user == null)
            {
                return NotFound(new ResponseViewModel(404, "Something going wrong."));
            }

            var userLanguage = new List<AppUserDtoLanguageDto>();
            foreach (var item in formdata.Languages)
            {
                userLanguage.Add(new AppUserDtoLanguageDto
                {
                    LanguageId = item.Id,
                    AppUserId = userId
                });
            }

            if (formdata.Photo != null)
            {
                user.Photo = await _azureBlobStorageService.UploadFileAsync(formdata.Photo);
            }
            user.UserName = formdata.UserName;
            user.GenderId = formdata.Gender.Id;
            user.DateOfBirth = Convert.ToDateTime(formdata.DateOfBirth);
            user.CountryId = formdata.Country.Id;
            user.AppUserLanguages = userLanguage;
            user.Steam = formdata.Steam;

            var result = await _appUserService.UpdateAsync(user);

            return result.Succeeded
                ? Ok(new ResponseViewModel(200, "User profile updated successfully."))
                : (IActionResult)BadRequest(new ResponseViewModel(500, "User profile updated faild."));
        }
    }
}
