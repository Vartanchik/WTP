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
using System.Web;
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

namespace WTP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class DefaultController : Controller
    {
        private readonly IAppUserDtoService _appUserDtoService;
        private readonly AppSettings _appSettings;

        public DefaultController(IAppUserDtoService appUserDtoService, IOptions<AppSettings> appSettings)
        {
            _appUserDtoService = appUserDtoService;
            _appSettings = appSettings.Value;
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> Test([FromBody] ForgotPasswordViewModel formData)
        {
            Log.Debug($"{this.ToString()}, action = Test - Came request");

            var user = await _appUserDtoService.GetByEmailAsync(formData.Email);
            Log.Debug($"{this.ToString()}, action = Test - User's email: {user.Email}");

            var token = await _appUserDtoService.GetPasswordResetTokenAsync(user);
            Log.Debug($"{this.ToString()}, action = Test - Token: {token}");

            //token = HttpUtility.UrlEncode(token);
            //Log.Debug($"{this.ToString()}, action = Test - TokenEncode: {token}");

            var callbackUrl = Url.Action("ResetPassword", "Account",
                new { userId = user.Id, code = token }, protocol: HttpContext.Request.Scheme);

            callbackUrl = callbackUrl.Substring(callbackUrl.IndexOf("code=") + 5);
            Log.Debug($"{this.ToString()}, action = Test - UrlCode: {callbackUrl}");

            //var tokenDecode = HttpUtility.UrlDecode(token);
            //Log.Debug($"{this.ToString()}, action = Test - TokenDecode: {tokenDecode}");

            var tokenDecode2 = HttpUtility.UrlDecode(callbackUrl);
            Log.Debug($"{this.ToString()}, action = Test - UrlDecode: {tokenDecode2}");


            return Ok();
        }
    }
}