using System;
using System.Web;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WTP.Logging;
using WTP.BLL.Services.Concrete.AppUserService;
using WTP.BLL.Services.EmailService;
using WTP.WebAPI.Utility.Extensions;
using WTP.BLL.DTOs.AppUserDTOs;
using WTP.BLL.DTOs.ServicesDTOs;

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
        /// <param name="dot"></param>
        [HttpPost("[action]")]
        [ProducesResponseType(typeof(ResponseDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> Register([FromBody] RegisterDto dot)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto(400, "Invalid value was entered! Please, redisplay form."));
            }

            var user = new BLL.DTOs.AppUserDTOs.AppUserDto
            {
                Email = dot.Email,
                UserName = dot.UserName,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _appUserService.CreateAsync(user, dot.Password);

            if (result.Succeeded)
            {
                await SendEmailConfirmationAsync(dot.Email);

                return Ok(new ResponseDto
                {
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
        [HttpGet("[action]")]
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
        /// <param name="dto"></param>
        [HttpPost("[action]")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResponseDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto(400, "Error", "Invalid value was entered! Please, redisplay form."));
            }

            var user = await _appUserService.GetByEmailAsync(dto.Email);

            if (user != null && await _appUserService.IsEmailConfirmedAsync(user.Id))
            {
                await SendResetPasswordEmailAsync(user);
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
        /// <param name="dto"></param>
        [HttpPost("[action]")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResponseDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto(400, "Invalid value was entered! Please, redisplay form."));
            }

            var token = HttpUtility.UrlDecode(dto.Token);
            var result = await _appUserService.ResetPasswordAsync(new ResetPasswordDto(dto.Id,
                                                                                       token,
                                                                                       dto.NewPassword));

            return result.Succeeded
                ? Ok(new ResponseDto(200, "Completed.", "Password reset is successful."))
                : (IActionResult)BadRequest(new ResponseDto(400, "Password reset is failed.", "Something going wrong."));
        }

        /// <summary>
        /// Change password of current user
        /// </summary>
        /// <param name="dto"></param>
        [HttpPost("[action]")]
        [Authorize(Policy = "RequireLoggedIn")]
        [ProducesResponseType(typeof(ResponseDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto(400, "Invalid value was entered! Please, redisplay form."));
            }

            int userId = this.GetCurrentUserId();

            var result = await _appUserService.ChangePasswordAsync(new ChangePasswordDto(userId,
                                                                                         dto.CurrentPassword,
                                                                                         dto.NewPassword));

            return result.Succeeded
                ? Ok(new ResponseDto(200, "Completed.", "Password update successful."))
                : (IActionResult)BadRequest(new ResponseDto(500, "Change password failed.", result.Errors.First().Description));
        }

        private async Task SendEmailConfirmationAsync(string email)
        {
            var userForConfirmEmail = await _appUserService.GetByEmailAsync(email);

            var token = await _appUserService.GenerateEmailConfirmationTokenAsync(userForConfirmEmail);

            var callbackUrl = Url.Action(
                "ConfirmEmail",
                "Account",
                new { userId = userForConfirmEmail.Id, token }, protocol: HttpContext.Request.Scheme);

            var emailConfigDto = new EmailConfigDto(_configuration["Email:Email"],
                                                    _configuration["Email:Password"],
                                                    _configuration["Email:Host"],
                                                    _configuration["Email:Port"]);

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

            var emailConfigDto = new EmailConfigDto(_configuration["Email:Email"],
                                                    _configuration["Email:Password"],
                                                    _configuration["Email:Host"],
                                                    _configuration["Email:Port"]);

            await _emailService.SendEmailAsync(
                user.Email,
                "WTP Password Reset",
                $"If You want to reset Your password, follow this: <a href='{callbackUrl}'>link</a>",
                emailConfigDto
                );
        }

        /// <summary>
        /// Delete current user account
        /// </summary>
        /// <returns></returns>
        [HttpDelete("[action]")]
        [Authorize(Policy = "RequireLoggedIn")]
        [ProducesResponseType(typeof(ResponseDto), 200)]
        public async Task<IActionResult> Delete()
        {
            var userId = this.GetCurrentUserId();

            await _appUserService.DeleteAccountAsync(userId);

            return Ok(new ResponseDto(200, "Completed.", "Account has been successfully deleted."));
        }

        [HttpPost("[action]")]
        [ProducesResponseType(typeof(ResponseDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> Restore(string email)
        {
            var userToRestore = await _appUserService.GetByEmailAsync(email);

            if (userToRestore == null)
            {
                return BadRequest(new ResponseDto(400, "Faild.", "No user found by this mail."));
            }

            var token = await _appUserService.CreateRestoreAccountToken(userToRestore.Id);

            var callbackUrl = Url.Action(
                "Restore",
                "Account",
                new { userId = userToRestore.Id, token }, protocol: HttpContext.Request.Scheme);

            var emailConfigDto = new EmailConfigDto(_configuration["Email:Email"],
                                                    _configuration["Email:Password"],
                                                    _configuration["Email:Host"],
                                                    _configuration["Email:Port"]);

            await _emailService.SendEmailAsync(
                email,
                "Just one click and you're on WTP",
                $"You can restore your account. Just use this link to enjoy: " +
                $"<a href='{callbackUrl}'>Account restore</a>.",
                emailConfigDto);

            return Ok(new ResponseDto(200, "Completed.", "Follow the instructions in the email."));
        }

        /// <summary>
        /// Restore your account by id & restore token
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        [HttpGet("[action]")]
        [ProducesResponseType(typeof(ResponseDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> Restore([FromQuery] string userId, [FromQuery] string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return BadRequest(new ResponseDto(400, "Faild.", "Account restore faild."));
            }

            var id = Convert.ToInt32(userId);

            var result = await _appUserService.RestoreAccountAsync(id, token);

            return result
                ? Ok(new ResponseDto(200, "Completed.", "Account has been successfully restore."))
                : (IActionResult)BadRequest(new ResponseDto(400, "Faild.", "Account restore faild."));
        }
    }
}
