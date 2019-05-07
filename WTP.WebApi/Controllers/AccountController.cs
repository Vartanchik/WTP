using System;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WTP.Logging;
using WTP.BLL.ModelsDto.AppUser;
using WTP.BLL.Services.Concrete.AppUserService;
using WTP.BLL.Services.Concrete.EmailService;
using WTP.WebAPI.Utility.Extensions;
using WTP.WebAPI.ViewModels;
using System.Linq;
using Microsoft.Extensions.Configuration;

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

        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel formdata)
        {
            var user = new AppUserDto
            {
                Email = formdata.Email,
                UserName = formdata.UserName,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _appUserService.CreateAsync(user, formdata.Password);

            if (result.Succeeded)
            {
                var userForConfirmEmail = await _appUserService.GetByEmailAsync(formdata.Email);

                var code = await _appUserService.GenerateEmailConfirmationTokenAsync(userForConfirmEmail);

                var callbackUrl = Url.Action(
                    "ConfirmEmail",
                    "Account",
                    new { userId = userForConfirmEmail.Id, code },
                    protocol: HttpContext.Request.Scheme);

                await _emailService.SendEmailAsync(formdata.Email, "Just one click and you're on WTP",
                    $"Thanks for registering to be a part of evolving esports with WTP. After you: <a href='{callbackUrl}'>confirm your email</a> you'll be able to enjoy all the benefits of the WTP platform.");

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
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return Redirect($"{_configuration["Url:BaseUrl"]}/home?confirmed=false");
            }

            var user = await _appUserService.FindByIdAsync(userId);
            if (user == null)
            {
                return Redirect($"{_configuration["Url:BaseUrl"]}/home?confirmed=false");
            }

            var result = await _appUserService.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return Redirect($"{_configuration["Url:BaseUrl"]}/home?confirmed=true");
            }

            return Redirect($"{_configuration["Url:BaseUrl"]}/home?confirmed=false");
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseViewModel(400, "Invalid value was entered! Please, redisplay form."));
            }

            var user = await _appUserService.GetByEmailAsync(formData.Email);

            if (user != null || await _appUserService.IsEmailConfirmedAsync(user))
            {

                var token = await _appUserService.GeneratePasswordResetTokenAsync(user);

                token = HttpUtility.UrlEncode(token);

                var callbackUrl = Url.Action("ResetPassword", "Account",
                    new { userId = user.Id, code = token }, protocol: HttpContext.Request.Scheme);

                await _emailService.SendEmailAsync(
                    formData.Email,
                    "WTP Password Reset",
                    $"If You want to reset Your password, follow this: <a href='{callbackUrl}'>link</a>");
            }

            return Ok(new ResponseViewModel
            {
                StatusCode = 200,
                Message = "Instructions are sent. Please, check Your email.",
                Info = "If there is no user with such email, or email is not confirmed - the letter won\'t be delivered!"
            });
        }

        [HttpGet("[action]")]
        [AllowAnonymous]
        public IActionResult ResetPassword([FromQuery] string userId = null, [FromQuery] string code = null)
        {
            return userId == null || code == null
                ? Redirect($"{_configuration["Url:BaseUrl"]}/account/forgot-password?resetIsFailed=true")
                : Redirect($"{_configuration["Url:BaseUrl"]}/account/reset-password?userId={userId}&code={code}");
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseViewModel(400, "Invalid value was entered! Please, redisplay form."));
            }

            var user = await _appUserService.GetAsync(formData.Id);

            var result = await _appUserService.ResetPasswordAsync(user, formData.Code, formData.NewPassword);

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

            var appUserDto = await _appUserService.GetAsync(userId);

            if (appUserDto == null)
            {
                return NotFound(new ResponseViewModel(404, "Something going wrong."));
            }

            var isPasswordValid = await _appUserService.CheckPasswordAsync(userId, formdata.CurrentPassword);

            if (isPasswordValid != true)
            {
                return BadRequest(new ResponseViewModel(400, "Invalid current password."));
            }

            var result = await _appUserService.ChangePasswordAsync(new ChangePasswordDto
            {
                UserId = userId,
                CurrentPassword = formdata.CurrentPassword,
                NewPassword = formdata.NewPassword
            });

            return result.Succeeded 
                ? Ok(new ResponseViewModel(200, "Password update successful."))
                : (IActionResult)BadRequest(new ResponseViewModel(500, "Change password failed."));
        }
    }
}
