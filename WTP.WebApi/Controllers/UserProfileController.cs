using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WTP.BLL.ModelsDto.AppUserLanguage;
using WTP.BLL.ModelsDto.Language;
using WTP.BLL.Services.Concrete.AppUserService;

namespace WTP.WebAPI.ViewModels.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IAppUserService _appUserService;
        
        public UserProfileController(IAppUserService appUserService)
        {
            _appUserService = appUserService;
        }

        //GET : /api/UserProfile
        [HttpGet]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> GetUserProfile()
        {
            int userId = Convert.ToInt32(User.Claims.First(c => c.Type == "UserID").Value);

            var user = await _appUserService.GetAsync(userId);

            var langs = new List<LanguageDto>();
            foreach(var item in user.AppUserLanguages)
            {
                langs.Add(new LanguageDto
                {
                    Id = item.LanguageId,
                    Name = item.Language.Name
                });
            }
            // Convert userDto to appUserDtoViewModel
            var appUserDtoViewModel = new AppUserDtoViewModel()
            {
                Email = user.Email,
                Photo = user.Photo,
                UserName = user.UserName,
                Gender = user.Gender,
                DateOfBirth = user.DateOfBirth,
                Languages = langs,
                Country = user.Country,
                Steam = user.Steam,
                Players = user.Players,
                Teams = user.Teams
            };

            return Ok(appUserDtoViewModel);
        }

        //PUT : /api/UserProfile
        [HttpPut]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> UpdateUserProfile([FromBody] AppUserDtoViewModel formdata)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Will hold all the errors related to registration
            List<string> errorList = new List<string>();

            int userId = Convert.ToInt32(User.Claims.First(c => c.Type == "UserID").Value);

            var user = await _appUserService.GetAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            //Update languages
            var languages = new List<AppUserDtoLanguageDto>();
            foreach (var item in formdata.Languages)
            {
                languages.Add(new AppUserDtoLanguageDto
                {
                    LanguageId = item.Id,
                    AppUserId = userId
                });
            }

            // If the user was found
            if (formdata.Photo != null)
            {
                user.Photo = formdata.Photo;
            }
            user.UserName = formdata.UserName;
            user.GenderId = formdata.Gender.Id;
            user.DateOfBirth = formdata.DateOfBirth;
            user.CountryId = formdata.Country.Id;
            user.AppUserLanguages = languages;
            user.Steam = formdata.Steam;

            var result = await _appUserService.UpdateAsync(user);

            if (result.Succeeded)
            {
                var appUserDtoViewModel = new AppUserDtoViewModel()
                {
                    Photo = user.Photo,
                    UserName = user.UserName
                };

                return Ok(appUserDtoViewModel);
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    errorList.Add(error.Code);
                }
            }

            return Ok(new ErrorResponseModel { Message = errorList });
        }
    }
}