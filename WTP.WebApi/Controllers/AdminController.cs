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
    //[Authorize(Roles = "Admin")]
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
        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel formdata)
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

        //GetAllUsers
        [HttpGet]
        [Route("GetUserProfile")]
        //[Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> GetUserProfile()
        {
            var users = await _appUserService.GetAllAsync();
            if (users.Count() == 0)
                return Ok("List of Users are empty!");
            List<AppUserDtoViewModel> result = new List<AppUserDtoViewModel>();
            //int userId = 1;            
            foreach (var t in users)
            {
                var user = t;//await _appUserService.GetAsync(userId);
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

                result.Add(appUserDtoViewModel);
            }
            return Ok(new JsonResult(result));
        }
    }
}
