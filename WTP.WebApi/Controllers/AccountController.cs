﻿using System;
using System.Web;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WTP.Logging;
using WTP.BLL.ModelsDto.AppUser;
using WTP.BLL.Services.Concrete.AppUserService;
using WTP.BLL.Services.Concrete.EmailService;
using WTP.WebAPI.Utility.Extensions;
using WTP.WebAPI.ViewModels;
using AutoMapper;
using WTP.BLL.ModelsDto.Email;

namespace WTP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IAppUserService _appUserService;
        private readonly IEmailService _emailService;
        private readonly ILog _log;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AccountController(IAppUserService appUserService, IEmailService emailService, ILog log, IConfiguration configuration, IMapper mapper)
        {
            _emailService = emailService;
            _log = log;
            _appUserService = appUserService;
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseViewModel(400, "Invalid value was entered! Please, redisplay form."));
            }

            var user = new AppUserDto
            {
                Email = formData.Email,
                UserName = formData.UserName,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _appUserService.CreateAsync(user, formData.Password);

            if (result.Succeeded)
            {
                await SendEmailConfirmationAsync(formData.Email);

                return Ok(new ResponseViewModel {
                    StatusCode = 200,
                    Message = "Registration is successful.",
                    Info = "To complete the registration, check the email and click on the link indicated in the letter."
                });
            }

            var errorInfo = result.Errors.First(err => err.Code == "DuplicateUserName" || err.Code == "DuplicateEmail");

            return BadRequest(new ResponseViewModel(400, "Registration is faild.", errorInfo.Description));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return Redirect($"{_configuration["Url:BaseUrl"]}/home?confirmed=false");
            }

            var user = await _appUserService.FindByIdAsync(userId);

            if (user == null)
            {
                return Redirect($"{_configuration["Url:BaseUrl"]}/home?confirmed=false");
            }

            var result = await _appUserService.ConfirmEmailAsync(user, token);

            return result.Succeeded
                ? Redirect($"{_configuration["Url:BaseUrl"]}/home?confirmed=true")
                : Redirect($"{_configuration["Url:BaseUrl"]}/home?confirmed=false");
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseViewModel(400, "Error", "Invalid value was entered! Please, redisplay form."));
            }

            var user = await _appUserService.GetByEmailAsync(formData.Email);

            if (user != null && await _appUserService.IsEmailConfirmedAsync(user))
            {
                await SendResetPasswordEmailAsync(user);
            }

            return Ok(new ResponseViewModel(200,
                "Instructions are sent. Please, check Your email.",
                "If there is no user with such email, or email is not confirmed - the letter won\'t be delivered!"
                ));
        }

        [HttpGet("[action]")]
        [AllowAnonymous]
        public IActionResult ResetPassword([FromQuery] string userId = null, [FromQuery] string code = null)
        {
            if(userId == null || code == null)
            {
                return Redirect($"{_configuration["Url:BaseUrl"]}/account/forgot-password?resetIsFailed=true");
            }

            code = HttpUtility.UrlEncode(code);

            return Redirect($"{_configuration["Url:BaseUrl"]}/account/reset-password?userId={userId}&code={code}");
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseViewModel(400, "Invalid value was entered! Please, redisplay form."));
            }

            var token = HttpUtility.UrlDecode(formData.Code);

            var user = await _appUserService.GetAsync(formData.Id);

            var result = await _appUserService.ResetPasswordAsync(user, token, formData.NewPassword);

            return result.Succeeded
                ? Ok(new ResponseViewModel(200, "Password reset is successful!"))
                : (IActionResult)BadRequest(new ResponseViewModel(500, "Password reset is failed!"));
        }

        [HttpPost("[action]")]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel formdata)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseViewModel(400, "Invalid value was entered! Please, redisplay form."));
            }

            int userId = this.GetCurrentUserId();

            var result = await _appUserService.ChangePasswordAsync(new ChangePasswordDto(userId,
                                                                                         formdata.CurrentPassword,
                                                                                         formdata.NewPassword));

            return result.Succeeded 
                ? Ok(new ResponseViewModel(200, "Password update successful."))
                : (IActionResult)BadRequest(new ResponseViewModel(500, "Change password failed.", result.Errors.First().Description));
        }

        private async Task SendEmailConfirmationAsync(string email)
        {
            var userForConfirmEmail = await _appUserService.GetByEmailAsync(email);

            var token = await _appUserService.GenerateEmailConfirmationTokenAsync(userForConfirmEmail);

            var callbackUrl = Url.Action(
                "ConfirmEmail",
                "Account",
                new { userId = userForConfirmEmail.Id, token }, protocol: HttpContext.Request.Scheme);

            var emailConfigDto = _mapper.Map<BLL.ModelsDto.Email.EmailConfigModel>(new ViewModels.EmailConfigDto(_configuration));

            await _emailService.SendEmailAsync(
                email,
                "Just one click and you're on WTP",
                $"Thanks for registering to be a part of evolving esports with WTP. After you: " +
                $"<a href='{callbackUrl}'>confirm your email</a> you'll be able to enjoy all the benefits of the WTP platform.",
                emailConfigDto
                );
        }

        private async Task SendResetPasswordEmailAsync(AppUserDto user)
        {
            var token = await _appUserService.GeneratePasswordResetTokenAsync(user);

            var callbackUrl = Url.Action("ResetPassword", "Account",
                new { userId = user.Id, code = token }, protocol: HttpContext.Request.Scheme);

            var emailConfigDto = _mapper.Map<BLL.ModelsDto.Email.EmailConfigModel>(new ViewModels.EmailConfigDto(_configuration));

            await _emailService.SendEmailAsync(
                user.Email,
                "WTP Password Reset",
                $"If You want to reset Your password, follow this: <a href='{callbackUrl}'>link</a>",
                emailConfigDto
                );
        }
    }
}
