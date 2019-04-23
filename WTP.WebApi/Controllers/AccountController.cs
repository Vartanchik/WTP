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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordViewModel formData)
        {
            var user = await _appUserDtoService.GetByEmailAsync(formData.Email);

            EmailService emailService = new EmailService();

            if (user == null)
            {
                // Sending Email about failed procedure 
                emailService.SendEmail(
                    formData.Email,
                    "WTP Password Reset",
                    @"<h1>Unfortunately the user with such Email is not found.</h1><br><a href=''>Please, try again!</a>");

                return Ok();
            }
            else
            {
                //byte[] recoveryToken = new byte[16];
                //var rng = new RNGCryptoServiceProvider();
                //rng.GetBytes(recoveryToken);
                //byte[] hashedRecoveryToken = new SHA256CryptoServiceProvider().ComputeHash(recoveryToken);
                //var key = BitConverter.ToString(hashedRecoveryToken);

                var key = await _appUserDtoService.GetPasswordResetTokenAsync(user);

                var callbackUrl = Url.Action("ResetPassword", "Account", new { userID = user.Id, code = key }, protocol: HttpContext.Request.Scheme);

                emailService.SendEmail(
                    formData.Email,
                    "WTP Password Reset",
                    $@"<h1>Click here to reset your password:</h1><br><a href='{callbackUrl}'>link</a>");

                return Ok();
            }
        }

        //[HttpGet("[action]")]
        //[AllowAnonymous]
        //public IActionResult ResetPassword(string code = null)
        //{
        //    return code == null ? View("Error") : View();
        //}

        //[HttpPost("[action]")]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordViewModel formData)
        //{
        //    var user = await _appUserDtoService.GetAsync(formData.);
        //}
    }


    public class EmailService
    {
        public void SendEmail(string email, string subject, string message)
        {
            var emailMessage = new MailMessage("avg0test0@gmail.com", email);
            emailMessage.Subject = subject;
            emailMessage.IsBodyHtml = true;
            emailMessage.Body = message;

            using (var client = new SmtpClient())
            {
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("avg0test0@gmail.com", "TeSt159357");
                client.Send(emailMessage);
            }
        }
    }
}