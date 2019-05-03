﻿using System;
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
using Microsoft.AspNetCore.Authorization;
using System.Web;
using WTP.BLL.Services.Concrete.EmailService;
using WTP.WebAPI.Utility.Extensions;

namespace WTP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IAppUserService _appUserService;
        private readonly IEmailService _emailService;
        private readonly ILog _log;

        public AccountController(IAppUserService appUserService, IEmailService emailService, ILog log)
        {
            _emailService = emailService;
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
                var userForConfirmEmail = await _appUserService.GetByEmailAsync(formdata.Email);

                var code = await _appUserService.GenerateEmailConfirmationTokenAsync(userForConfirmEmail);
                _log.Debug($"\nUser was created. {this.ToString()}, action = Register.Token = {code}");

                var callbackUrl = Url.Action(
                    "ConfirmEmail",
                    "Account",
                    new { userId = userForConfirmEmail.Id, code },
                    protocol: HttpContext.Request.Scheme);

                await _emailService.SendEmailAsync(formdata.Email, "Just one click and you're on WTP",
                    $"Thanks for registering to be a part of evolving esports with WTP. After you: <a href='{callbackUrl}'>confirm your email</a> you'll be able to enjoy all the benefits of the WTP platform.");

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

            return Ok(new ErrorResponseModel { Message = errorList });
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return Redirect("http://localhost:4200/home?confirmed=false");
            }

            var user = await _appUserService.FindByIdAsync(userId);
            if (user == null)
            {
                return Redirect("http://localhost:4200/home?confirmed=false");
            }

            var result = await _appUserService.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return Redirect("http://localhost:4200/home?confirmed=true");
            }

            return Redirect("http://localhost:4200/home?confirmed=false");
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordViewModel formData)
        {
            _log.Debug($"{this.ToString()}, action = ForgotPassword HttpPost. Came request");

            List<string> errorList = new List<string>();

            if (ModelState.IsValid)
            {
                _log.Debug($"{this.ToString()}, action = ForgotPassword HttpPost. Model state is valid");

                var user = await _appUserService.GetByEmailAsync(formData.Email);

                if (user == null || !(await _appUserService.IsEmailConfirmedAsync(user)))
                {
                    _log.Debug($"{this.ToString()}, action = ForgotPassword HttpPost. User wasn't found or email wasn't confirmed");

                    return Ok();
                }
                else
                {
                    _log.Debug($"{this.ToString()}, action = ForgotPassword HttpPost. User was found, user's hashcode: {user.GetHashCode()}");

                    var token = await _appUserService.GeneratePasswordResetTokenAsync(user);
                    _log.Debug($"{this.ToString()}, action = ForgotPassword HttpPost. Token was generated: {token}");

                    token = HttpUtility.UrlEncode(token);
                    _log.Debug($"{this.ToString()}, action = ForgotPassword HttpPost. Token was encoded: {token}");

                    var callbackUrl = Url.Action("ResetPassword", "Account",
                        new { userId = user.Id, code = token }, protocol: HttpContext.Request.Scheme);
                    _log.Debug($"{this.ToString()}, action = ForgotPassword HttpPost. Token in URL: {callbackUrl.Substring(callbackUrl.IndexOf("code=") + 5)}");

                    await _emailService.SendEmailAsync(
                        formData.Email,
                        "WTP Password Reset",
                        $"If You want to reset Your password, follow this: <a href='{callbackUrl}'>link</a>");
                    _log.Debug($"{this.ToString()}, action = ForgotPassword HttpPost. Email with link was sent");

                    return Ok();
                }
            }
            _log.Debug($"{this.ToString()}, action = ForgotPassword HttpPost. Model state is invalid");

            errorList.Add("Invalid value was entered! Please, redisplay form.");
            return BadRequest(new JsonResult(errorList));
        }

        [HttpGet("[action]")]
        [AllowAnonymous]
        public IActionResult ResetPassword([FromQuery] string userId = null, [FromQuery] string code = null)
        {
            _log.Debug($"{this.ToString()}, action = ResetPassword HttpGET. Came request");

            if (userId == null || code == null)
            {
                _log.Debug($"{this.ToString()}, action = ResetPassword HttpGET. userId={userId} or code={code} is null");

                return Redirect("http://localhost:4200/account/forgot-password?fell=true");
            }
            _log.Debug($"{this.ToString()}, action = ResetPassword HttpGET. userId={userId} and code={code} are not null");

            return Redirect($"http://localhost:4200/account/reset-password?userId={userId}&code={code}");
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordViewModel formData)
        {
            _log.Debug($"{this.ToString()}, action = ResetPassword HttpPost. Came request");

            List<string> errorList = new List<string>();

            if (ModelState.IsValid)
            {
                _log.Debug($"{this.ToString()}, action = ResetPassword HttpPost. Model state is valid");
                _log.Debug($"{this.ToString()}, action = ResetPassword HttpPost. Token: {formData.Code}");

                var user = await _appUserService.GetAsync(formData.Id);
                _log.Debug($"{this.ToString()}, action = ResetPassword HttpPost. User was found");

                var result = await _appUserService.ResetPasswordAsync(user, formData.Code, formData.Password);

                if (result.Succeeded)
                {
                    _log.Debug($"{this.ToString()}, action = ResetPassword HttpPost. ResetPassword is succeed");

                    return Ok(result);
                }
                else
                {
                    _log.Debug($"{this.ToString()}, action = ResetPassword HttpPost. ResetPassword isn't succeed");

                    foreach (var error in result.Errors)
                    {
                        errorList.Add(error.Code);
                    }
                }

                foreach (var err in errorList)
                {
                    _log.Debug($"{ this.ToString()}, action = ResetPassword HttpPost. Error: {err}");
                }
                return BadRequest(new JsonResult(errorList));
            }
            _log.Debug($"{this.ToString()}, action = ResetPassword HttpPost. Model state isn't valid");

            errorList.Add("Invalid value was entered! Please, redisplay form.");
            return BadRequest(new JsonResult(errorList));
        }

        //POST : /api/UserProfile
        [HttpPost("[action]")]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel formdata)
        {
            #region Validation
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseViewModel
                {
                    StatusCode = 400,
                    Message = "Syntax error."
                });
            }

            if (formdata.CurrentPassword == formdata.NewPassword)
            {
                return BadRequest(new ResponseViewModel
                {
                    StatusCode = 400,
                    Message = "You can't change password for the current one."
                });
            }
            #endregion

            #region Get User
            int userId = this.GetCurrentUserId();

            var appUserDto = await _appUserService.GetAsync(userId);

            if (appUserDto == null)
            {
                return NotFound(new ResponseViewModel
                {
                    StatusCode = 404,
                    Message = "User not found."
                });
            }
            #endregion

            #region Change Password
            var result = await _appUserService.ChangePasswordAsync(new ChangePasswordDto
            {
                UserId = userId,
                CurrentPassword = formdata.CurrentPassword,
                NewPassword = formdata.NewPassword
            });
            #endregion

            #region Result
            if (result.Succeeded)
            {
                return Ok(new ResponseViewModel
                {
                    StatusCode = 200,
                    Message = "Password update successful."
                });
            }

            return BadRequest(new ResponseViewModel
            {
                StatusCode = 500,
                Message = result.ToString() //"Server error."
            });
            #endregion
        }
    }
}
