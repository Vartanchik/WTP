using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WTP.BLL.ModelsDto;
using WTP.BLL.Services.AppUserDtoService;
using WTP.BLL.TransferModels;
using WTP.WebAPI.ViewModels;

namespace GamePlatform_WebAPI.BusinessLogicLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IAppUserDtoService _appUserDtoService;
        
        public UserProfileController(IAppUserDtoService appUserDtoService)
        {
            _appUserDtoService = appUserDtoService;
        }

        //GET : /api/UserProfile
        [HttpGet]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> GetUserProfile()
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;

            var user = await _appUserDtoService.GetAsync(userId);

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

            string userId = User.Claims.First(c => c.Type == "UserID").Value;

            var user = await _appUserDtoService.GetAsync(userId);

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

            var result = await _appUserDtoService.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok(new JsonResult("Updated"));
            }

            return BadRequest(new JsonResult("Not updated"));
        }
    }
}