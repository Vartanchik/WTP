using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WTP.BLL.ModelsDto.Admin;
using WTP.BLL.ModelsDto.AppUser;
using WTP.BLL.ModelsDto.AppUserLanguage;
using WTP.BLL.ModelsDto.Language;
using WTP.BLL.Services.Concrete.AdminService;
using WTP.BLL.Services.Concrete.AppUserService;
using WTP.BLL.Services.Concrete.HistoryService;
using WTP.Logging;
using WTP.WebAPI.Helpers;
using WTP.WebAPI.ViewModels;

namespace WTP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : Controller
    {
        //private readonly IHistoryService _historyService;
        private readonly AppSettings _appSettings;
        private readonly ILog _log;
        private readonly IAppUserService _appUserService;

        public AdminController(IOptions<AppSettings> appSettings, ILog log, IAppUserService appUserService)
        {
            _log = log;
            _appSettings = appSettings.Value;
            _appUserService = appUserService;
        }


        //Create Admin account
        [HttpPost]
        [Route("Admins/CreareProfile")]
        [Authorize(Policy = "RequireAdministratorRole")]
        public async Task<IActionResult> CreateAdminProfile([FromBody] RegisterViewModel formdata)
        {
            _log.Debug($"\nRequest to {this.ToString()}, action = Register");
            // Will hold all the errors related to registration
            List<string> errorList = new List<string>();

            var user = new AppUserDto
            {
                Email = formdata.Email,
                UserName = formdata.UserName,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _appUserService.CreateAdminAsync(user, formdata.Password);

            if (result.Succeeded)
            {
                _log.Debug($"\nUser was created. {this.ToString()}, action = Register");
                // Sending Confirmation Email

                return Ok(result);
            }
            else
            {
                _log.Debug($"\nUser wasn't created. {this.ToString()}, action = Register");
                foreach (var error in result.Errors)
                {
                    errorList.Add(error.Code);
                }
            }

            return BadRequest(new JsonResult(errorList));
        }

        //Get List of all Users
        [HttpGet]
        [Route("Users")]
        [Authorize(Policy = "RequireAdministratorRole")]
        public async Task<IActionResult> GetUsersProfile()
        {
            List<AppUserDtoViewModel> result = new List<AppUserDtoViewModel>();
            var users = await _appUserService.GetAllAsync();

            if (users.Count() == 0)
                return Ok("List of Users are empty!");
            
            foreach (var user in users)
            {
                var langs = new List<LanguageDto>();

                foreach (var item in user.AppUserLanguages)
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

                //Check if account isn't deleted
                if(user.DeletedStatus!=true)
                    result.Add(appUserDtoViewModel);
            }
            return Ok(new JsonResult(result));
            //return Ok();
        }

        //Update user's account
        [HttpPut]
        [Authorize(Policy = "RequireAdministratorRole")]
        [Route("Users/UpdateUser/{id}")]
        public async Task<IActionResult> UpdateUser([FromBody] AppUserDtoViewModel formdata,int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Will hold all the errors related to registration
            List<string> errorList = new List<string>();

            int userId = id;

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
                return Ok(new JsonResult("User with id "+user.Id+" was updated!"));
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    errorList.Add(error.Code);
                }
            }

            return BadRequest(new JsonResult(errorList));
        }

        //Delete user's account by id
        [HttpDelete]
        [Authorize(Policy = "RequireAdministratorRole")]
        [Route("Users/DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            bool status = await _appUserService.DeleteAsync(id);

            if (status)
                return Ok(new JsonResult("User with id " +id+" was deleted!"));

            return NotFound(new JsonResult("User with id " + id + " wasn't deleted!"));
            
        }

        //Lock users account by id
        [HttpPut]
        [Authorize(Policy = "RequireAdministratorRole")]
        [Route("Users/LockUser/{id}")]
        public async Task<IActionResult> LockUser([FromBody]LockViewModel formDate,int id)
        {
            bool status = await _appUserService.LockAsync(id,formDate.Days);

            if (status)
                return Ok(new JsonResult("User with id " + id + " was lock!"));
        
            return NotFound(new JsonResult("User with id " + id + " wasn't lock!"));

        }

        //UnLock user's account by id
        [HttpPut]
        [Authorize(Policy = "RequireAdministratorRole")]
        [Route("Users/UnLockUser/{id}")]
        public async Task<IActionResult> UnLockUser(int id)
        {
            bool status = await _appUserService.UnLockAsync(id);

            if (status)            
                return Ok(new JsonResult("User with id " + id + " was unlock!"));

            return NotFound(new JsonResult("User with id " + id + " wasn't unlock!"));

        }
    }
}
