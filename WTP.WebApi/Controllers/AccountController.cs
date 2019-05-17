﻿using System;
using System.Web;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WTP.Logging;
using WTP.BLL.Dto.AppUser;
using WTP.BLL.Services.Concrete.AppUserService;
using WTP.BLL.Services.EmailService;
using WTP.WebAPI.Utility.Extensions;
using WTP.WebAPI.Models;
using AutoMapper;
using WTP.BLL.Dto.Email;

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

        /// <summary>
        /// Registration of new user
        /// </summary>
        /// <param name="formData"></param>
        /// <response code="200">Successful performance</response>
        /// <response code="400">The action failed</response>
        [HttpPost("[action]")]
        [ProducesResponseType(typeof(ResponseModel), 200)]
        [ProducesResponseType(typeof(ResponseModel), 400)]
        public async Task<IActionResult> Register([FromBody] RegisterModel formData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseModel(400, "Invalid value was entered! Please, redisplay form."));
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

                return Ok(new ResponseModel
                {
                    StatusCode = 200,
                    Message = "Registration is successful.",
                    Info = "To complete the registration, check the email and click on the link indicated in the letter."
                });
            }

            var errorInfo = result.Errors.First(err => err.Code == "DuplicateUserName" || err.Code == "DuplicateEmail");

            return BadRequest(new ResponseModel(400, "Registration is faild.", errorInfo.Description));
        }

        /// <summary>
        /// Confirmation user email
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <response code="302">Successful performance</response>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(302)]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var result = await _appUserService.ConfirmEmailAsync(Convert.ToInt32(userId), token);

            return result.Succeeded
                ? Redirect($"{_configuration["Url:BaseUrl"]}/home?confirmed=true")
                : Redirect($"{_configuration["Url:BaseUrl"]}/home?confirmed=false");
        }

        /// <summary>
        /// Send email for reset password
        /// </summary>
        /// <param name="formData"></param>
        /// <returns>ResponseModel</returns>
        /// <response code="200">Successful performance</response>
        [HttpPost("[action]")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResponseModel), 200)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel formData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseModel(400, "Error", "Invalid value was entered! Please, redisplay form."));
            }

            var user = await _appUserService.GetByEmailAsync(formData.Email);

            if (user != null && await _appUserService.IsEmailConfirmedAsync(user.Id))
            {
                await SendResetPasswordEmailAsync(user);
            }

            return Ok(new ResponseModel(200,
                "Instructions are sent. Please, check Your email.",
                "If there is no user with such email, or email is not confirmed - the letter won\'t be delivered!"
                ));
        }

        /// <summary>
        /// Redirect to frontend
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="code"></param>
        /// <response code="302">Successful performance</response>
        [HttpGet("[action]")]
        [AllowAnonymous]
        [ProducesResponseType(302)]
        public IActionResult ResetPassword([FromQuery] string userId = null, [FromQuery] string code = null)
        {
            if (userId == null || code == null)
            {
                return Redirect($"{_configuration["Url:BaseUrl"]}/account/forgot-password?resetIsFailed=true");
            }

            code = HttpUtility.UrlEncode(code);

            return Redirect($"{_configuration["Url:BaseUrl"]}/account/reset-password?userId={userId}&code={code}");
        }

        /// <summary>
        /// Reset user password by email 
        /// </summary>
        /// <param name="formData"></param>
        /// <returns>ResponseModel</returns>
        /// <response code="200">Successful performance</response>
        /// <response code="400">The action failed</response>
        [HttpPost("[action]")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResponseModel), 200)]
        [ProducesResponseType(typeof(ResponseModel), 400)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel formData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseModel(400, "Invalid value was entered! Please, redisplay form."));
            }

            var token = HttpUtility.UrlDecode(formData.Code);

            var result = await _appUserService.ResetPasswordAsync(new ResetPasswordDto(formData.Id,
                                                                                       token,
                                                                                       formData.NewPassword));

            return result.Succeeded
                ? Ok(new ResponseModel(200, "Completed.", "Password reset is successful."))
                : (IActionResult)BadRequest(new ResponseModel(400, "Password reset is failed.", "Something going wrong."));
        }

        /// <summary>
        /// Change password of current user
        /// </summary>
        /// <param name="formdata"></param>
        /// <returns>ResponseModel</returns>
        /// <response code="200">Successful performance</response>
        /// <response code="500">The action failed</response>
        [HttpPost("[action]")]
        [Authorize(Policy = "RequireLoggedIn")]
        [ProducesResponseType(typeof(ResponseModel), 200)]
        [ProducesResponseType(typeof(ResponseModel), 500)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel formdata)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseModel(400, "Invalid value was entered! Please, redisplay form."));
            }

            int userId = this.GetCurrentUserId();

            var result = await _appUserService.ChangePasswordAsync(new ChangePasswordDto(userId,
                                                                                        formdata.CurrentPassword,
                                                                                        formdata.NewPassword));

            return result.Succeeded
                ? Ok(new ResponseModel(200, "Completed.", "Password update successful."))
                : (IActionResult)BadRequest(new ResponseModel(500, "Change password failed.", result.Errors.First().Description));
        }

        private async Task SendEmailConfirmationAsync(string email)
        {
            var userForConfirmEmail = await _appUserService.GetByEmailAsync(email);

            var token = await _appUserService.GenerateEmailConfirmationTokenAsync(userForConfirmEmail);

            var callbackUrl = Url.Action(
                "ConfirmEmail",
                "Account",
                new { userId = userForConfirmEmail.Id, token }, protocol: HttpContext.Request.Scheme);

            var emailConfigDto = _mapper.Map<EmailConfigDto>(new EmailConfigModel(_configuration));

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

            var emailConfigDto = _mapper.Map<EmailConfigDto>(new EmailConfigModel(_configuration));

            await _emailService.SendEmailAsync(
                user.Email,
                "WTP Password Reset",
                $"If You want to reset Your password, follow this: <a href='{callbackUrl}'>link</a>",
                emailConfigDto
                );
        }
    }
}
