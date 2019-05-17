using System;
using System.Web;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WTP.Logging;
using WTP.BLL.Models.AppUser;
using WTP.BLL.Services.Concrete.AppUserService;
using WTP.BLL.Services.Concrete.EmailService;
using WTP.WebAPI.Utility.Extensions;
using WTP.WebAPI.Dto;
using AutoMapper;

namespace WTP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IAppUserService _appUserService;
        private readonly IEmailService _emailService;
        private readonly ILog _log;
        private readonly IConfiguration _configuration;

        public AccountController(IAppUserService appUserService, IEmailService emailService, ILog log, IConfiguration configuration)
        {
            _emailService = emailService;
            _log = log;
            _appUserService = appUserService;
            _configuration = configuration;
        }

        /// <summary>
        /// Registration of new user
        /// </summary>
        /// <param name="formData"></param>
        /// <returns></returns>
        /// <response code="200">Successful performance</response>
        /// <response code="400">The action failed</response>
        [HttpPost("[action]")]
        [ProducesResponseType(typeof(ResponseDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> Register([FromBody] RegisterDto formData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto(400, "Invalid value was entered! Please, redisplay form."));
            }

            var user = new AppUserModel
            {
                Email = formData.Email,
                UserName = formData.UserName,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _appUserService.CreateAsync(user, formData.Password);

            if (result.Succeeded)
            {
                await SendEmailConfirmation(formData.Email);

                return Ok(new ResponseDto {
                    StatusCode = 200,
                    Message = "Registration is successful.",
                    Info = "To complete the registration, check the email and click on the link indicated in the letter."
                });
            }

            var errorInfo = result.Errors.First(err => err.Code == "DuplicateUserName" || err.Code == "DuplicateEmail");

            return BadRequest(new ResponseDto(400, "Registration is faild.", errorInfo.Description));
        }

        /// <summary>
        /// Confirmation user email
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
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
        /// Send email to reset password
        /// </summary>
        /// <param name="formData"></param>
        /// <returns></returns>
        /// <response code="200">Successful performance</response>
        /// <response code="400">The action failed</response>
        [HttpPost("[action]")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResponseDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto formData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto(400, "Error", "Invalid value was entered! Please, redisplay form."));
            }

            var user = await _appUserService.GetByEmailAsync(formData.Email);

            if (user != null && await _appUserService.IsEmailConfirmedAsync(user.Id))
            {
                await SendResetPassword(user);
            }

            return Ok(new ResponseDto(200,
                "Instructions are sent. Please, check Your email.",
                "If there is no user with such email, or email is not confirmed - the letter won\'t be delivered!"
                ));
        }

        /// <summary>
        /// No description
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        /// <response code="302">Successful performance</response>
        [HttpGet("[action]")]
        [AllowAnonymous]
        [ProducesResponseType(302)]
        public IActionResult ResetPassword([FromQuery] string userId = null, [FromQuery] string code = null)
        {
            return userId == null || code == null
                ? Redirect($"{_configuration["Url:BaseUrl"]}/account/forgot-password?resetIsFailed=true")
                : Redirect($"{_configuration["Url:BaseUrl"]}/account/reset-password?userId={userId}&code={code}");
        }

        /// <summary>
        /// Reset user password by email 
        /// </summary>
        /// <param name="formData"></param>
        /// <returns></returns>
        /// <response code="200">Successful performance</response>
        /// <response code="400">The action failed</response>
        [HttpPost("[action]")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResponseDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto formData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto(400, "Invalid value was entered! Please, redisplay form."));
            }

            var result = await _appUserService.ResetPasswordAsync(new ResetPasswordModel(formData.Id,
                                                                                         formData.Code,
                                                                                         formData.NewPassword));

            return result.Succeeded
                ? Ok(new ResponseDto(200, "Completed.", "Password reset is successful."))
                : (IActionResult)BadRequest(new ResponseDto(400, "Password reset is failed.", "Something going wrong."));
        }

        /// <summary>
        /// Change password of current user
        /// </summary>
        /// <param name="formdata"></param>
        /// <returns></returns>
        /// <response code="200">Successful performance</response>
        /// <response code="400">The action failed</response>
        [HttpPost("[action]")]
        [Authorize(Policy = "RequireLoggedIn")]
        [ProducesResponseType(typeof(ResponseDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto formdata)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto(400, "Invalid value was entered! Please, redisplay form."));
            }

            int userId = this.GetCurrentUserId();

            var result = await _appUserService.ChangePasswordAsync(new ChangePasswordModel(userId,
                                                                                           formdata.CurrentPassword,
                                                                                           formdata.NewPassword));

            return result.Succeeded 
                ? Ok(new ResponseDto(200, "Completed.", "Password update successful."))
                : (IActionResult)BadRequest(new ResponseDto(500, "Change password failed.", result.Errors.First().Description));
        }

        private async Task SendEmailConfirmation(string email)
        {
            var userForConfirmEmail = await _appUserService.GetByEmailAsync(email);

            var token = await _appUserService.GenerateEmailConfirmationTokenAsync(userForConfirmEmail);

            var callbackUrl = Url.Action(
                "ConfirmEmail",
                "Account",
                new { userId = userForConfirmEmail.Id, token }, protocol: HttpContext.Request.Scheme);

            await _emailService.SendEmailAsync(email, "Just one click and you're on WTP",
                $"Thanks for registering to be a part of evolving esports with WTP. After you: " +
                $"<a href='{callbackUrl}'>confirm your email</a> you'll be able to enjoy all the benefits of the WTP platform.");
        }

        private async Task SendResetPassword(AppUserModel user)
        {
            var token = await _appUserService.GeneratePasswordResetTokenAsync(user);

            token = HttpUtility.UrlEncode(token);

            var callbackUrl = Url.Action("ResetPassword", "Account",
                new { userId = user.Id, code = token }, protocol: HttpContext.Request.Scheme);

            await _emailService.SendEmailAsync(
                user.Email,
                "WTP Password Reset",
                $"If You want to reset Your password, follow this: <a href='{callbackUrl}'>link</a>");
        }
    }
}
