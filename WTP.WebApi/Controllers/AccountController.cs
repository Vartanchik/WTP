using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GamePlatform_WebAPI.BusinessLogicLayer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using WTP.BLL.Services.AppUserDtoService;
using WTP.BLL.TransferModels;
using WTP.WebApi.Helpers;
using WTP.WebAPI.ViewModels;

namespace GamePlatform_WebAPI.BusinessLogicLayer.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IAppUserDtoService _appUserDtoService;
        private readonly AppSettings _appSettings;

        public AccountController(IAppUserDtoService appUserDtoService, IOptions<AppSettings> appSettings)
        {
            _appUserDtoService = appUserDtoService;
            _appSettings = appSettings.Value;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel formdata)
        {
            // Will hold all the errors related to registration
            List<string> errorList = new List<string>();

            var user = new AppUserDto
            {
                Email = formdata.Email,
                UserName = formdata.UserName,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _appUserDtoService.CreateAsync(user, formdata.Password);

            if (result.Succeeded)
            {
                // Sending Confirmation Email

                return Ok(result);
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

        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel formdata)
        {
            // Get the User from Database
            var user = await _appUserDtoService.GetByEmailAsync(formdata.Email);

            var roles = await _appUserDtoService.GetRolesAsync(user);

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appSettings.Secret));

            double tokenExpiryTime = Convert.ToDouble(_appSettings.ExpireTime);

            if (user != null && await _appUserDtoService.CheckPasswordAsync(user.Id, formdata.Password))
            {
                // Confirmation of email

                var tokenHandler = new JwtSecurityTokenHandler();

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, formdata.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
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

                return Ok(new
                {
                    token = tokenHandler.WriteToken(token),
                    expiration = token.ValidTo,
                    username = user.UserName,
                    userRole = roles.FirstOrDefault()
                });
            }

            //return error
            ModelState.AddModelError("", "Username/Password was not Found");
            return Unauthorized(new { LoginError = "Please Check the Login Credentials - Ivalid Username/Password was entered" });
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordViewModel formData)
        {
            Log.Debug($"{this.ToString()}, action = ForgotPassword - Came request");

            if (ModelState.IsValid)
            {
                Log.Debug($"{this.ToString()}, action = ForgotPassword - Model state is valid");

                var user = await _appUserDtoService.GetByEmailAsync(formData.Email);
                Log.Debug($"{this.ToString()}, action = ForgotPassword - user={user.UserName}");

                var userEmailIsConfirmed = await _appUserDtoService.IsEmailConfirmedAsync(user);
                Log.Debug($"{this.ToString()}, action = ForgotPassword - userEmailIsConfirmed={userEmailIsConfirmed}");

                if (user == null /*|| !userEmailIsConfirmed*/)
                {
                    Log.Debug($"{this.ToString()}, action = ForgotPassword - User wasn't found");
                    // Sending Email about failed procedure 
                    await _appUserDtoService.SendEmailAsync(
                        formData.Email,
                        "WTP Password Reset",
                        @"<h2>Unfortunately the user with such Email wasn't found. Or Your Email isn't confirmed.</h2><br>
                    <h3>You can, <a href='http://localhost:4200/account/forgot-password'>try again</a></h3>");
                    Log.Debug($"{this.ToString()}, action = ForgotPassword - Email with rejection has been sent");

                    return Ok(new { succeeded = true });
                }

                else
                {
                    Log.Debug($"{this.ToString()}, action = ForgotPassword - User was found");

                    var token = await _appUserDtoService.GetPasswordResetTokenAsync(user);
                    Log.Debug($"{this.ToString()}, action = ForgotPassword - Token was generated {token}");

                    var callbackUrl = Url.Action("ResetPassword", "Account", 
                        new { userId = user.Id, code = token }, protocol: HttpContext.Request.Scheme);

                    await _appUserDtoService.SendEmailAsync(
                        formData.Email,
                        "WTP Password Reset",
                        $@"<h2>If You want to reset Your password, follow this link:</h2><br><a href='{callbackUrl}'>{callbackUrl}</a>");
                    Log.Debug($"{this.ToString()}, action = ForgotPassword - Email with link has been sent");

                    return Ok(new { succeeded = true });
                }
            }

            return BadRequest(new { LoginError = "Please, redisplay forgot password form - invalid value was entered!" });
        }

        [HttpGet("[action]")]
        [AllowAnonymous]
        public IActionResult ResetPassword([FromQuery] string userId = null, [FromQuery] string code = null)
        {
            Log.Debug($"{this.ToString()}, action = ResetPasswordGET - Came request");

            if (userId == null || code == null)
            {
                Log.Debug($"{this.ToString()}, action = ResetPasswordGET - userId={userId} or code={code} is null");

                return RedirectToPage(@"http://localhost:4200/account/forgot-password");
            }
            Log.Debug($"{this.ToString()}, action = ResetPasswordGET - userId={userId} and code={code} are not null");

            return Redirect($"http://localhost:4200/account/reset-password?id={userId}&token={code}");
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordViewModel formData)
        {
            Log.Debug($"{this.ToString()}, action = ResetPasswordPOST - Came request");

            List<string> errorList = new List<string>();

            if (ModelState.IsValid)
            {
                Log.Debug($"{this.ToString()}, action = ResetPasswordPOST - Model state is valid");

                var user = await _appUserDtoService.GetAsync(formData.Id);
                Log.Debug($"{this.ToString()}, action = ResetPasswordPOST - user={user}");

                var result = await _appUserDtoService.ResetPasswordAsync(user, formData.Code, formData.Password);

                if (result.Succeeded)
                {
                    Log.Debug($"{this.ToString()}, action = ResetPasswordPOST - ResetPassword is Succeeded");

                    return Ok(result);
                }
                else
                {
                    Log.Debug($"{this.ToString()}, action = ResetPasswordPOST - ResetPassword isn't Succeeded");

                    foreach (var error in result.Errors)
                    {
                        errorList.Add(error.Code);
                    }
                }
                return BadRequest(new JsonResult(errorList));
            }
            Log.Debug($"{this.ToString()}, action = ResetPasswordPOST - Model state isn't valid");

            return BadRequest(new { LoginError = "Please, redisplay reset password form - invalid value was entered!" });
        }
    }
}