using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WTP.Logging;
using WTP.BLL.ModelsDto.AppUser;
using WTP.BLL.Services.Concrete.AppUserService;
using WTP.WebAPI.Helpers;
using WTP.WebAPI.ViewModels;

namespace WTP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IAppUserService _appUserService;
        private readonly AppSettings _appSettings;
        private readonly ILog _log;

        public AccountController(IAppUserService appUserService, IOptions<AppSettings> appSettings, ILog log)
        {
            _appSettings = appSettings.Value;
            _log = log;
            _appUserService = appUserService;
        }

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

            var result = await _appUserService.CreateAsync(user, formdata.Password);

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

        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel formdata)
        {
            _log.Debug($"\nRequest to {this.ToString()}, action = Login");
            // Get the User from Database
            var user = await _appUserService.GetByEmailAsync(formdata.Email);

            var roles = await _appUserService.GetRolesAsync(user);

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appSettings.Secret));

            double tokenExpiryTime = Convert.ToDouble(_appSettings.ExpireTime);

            if (user != null && await _appUserService.CheckPasswordAsync(user.Id, formdata.Password))
            {
                _log.Debug($"\nUser was founded. {this.ToString()}, action = Login");
                // Confirmation of email

                var tokenHandler = new JwtSecurityTokenHandler();

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, formdata.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Role, roles.FirstOrDefault()),
                        new Claim("LoggedOn", DateTime.Now.ToString()),
                        new Claim("UserID", user.Id.ToString())
                    }),

                    SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
                    Issuer = _appSettings.Site,
                    Audience = _appSettings.Audience,
                    Expires = DateTime.UtcNow.AddDays(tokenExpiryTime)
                };

                // Generate Token
                var token = tokenHandler.CreateToken(tokenDescriptor);
                _log.Debug($"\nToken was created. {this.ToString()}, action = Login");

                return Ok(new
                {
                    token = tokenHandler.WriteToken(token),
                    expiration = token.ValidTo,
                    username = user.UserName,
                    userRole = roles.FirstOrDefault()
                });
            }
            _log.Debug($"\nUser wasn't founded. {this.ToString()}, action = Login");

            //return error
            ModelState.AddModelError("", "Username/Password was not Found");
            return Unauthorized(new { LoginError = "Please Check the Login Credentials - Ivalid Username/Password was entered" });
        }
    }
}