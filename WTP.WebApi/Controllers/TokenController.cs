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
using WTP.BLL.ModelsDto.AppUser;
using WTP.BLL.ModelsDto.RefreshToken;
using WTP.BLL.Services.Concrete.AppUserService;
using WTP.BLL.Services.Concrete.RefreshTokenService;
using WTP.WebAPI.Helpers;
using WTP.WebAPI.ViewModels;

namespace WTP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        //JWT and Refresh tokens

        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IAppUserService _appUserService;
        private readonly AppSettings _appSettings;

        public TokenController(IRefreshTokenService refreshTokenService, IAppUserService appUserService,
            IOptions<AppSettings> appSettings)
        {
            _refreshTokenService = refreshTokenService;
            _appUserService = appUserService;
            _appSettings = appSettings.Value;
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> Auth([FromBody] AccessRequestModel model) // granttype = "refresh_token or password"
        {
            // We will return Generic 500 HTTP Server Status Error
            // If we receive an invalid payload
            if (model == null)
            {
                return new StatusCodeResult(500);
            }

            switch (model.GrantType)
            {
                case "password":
                    return await Login(model);
                case "refresh_token":
                    return await UpdateAccessToken(model);
                default:
                    // not supported - return a HTTP 401 (Unauthorized)
                    return new UnauthorizedResult();
            }
        }

        // Method to Create New JWT and Refresh Token
        private async Task<IActionResult> Login(AccessRequestModel model)
        {
            var user = await _appUserService.GetByEmailAsync(model.Email);

            if (user == null && !await _appUserService.CheckPasswordAsync(user.Id, model.Password))
            {
                return BadRequest(new ResponseViewModel {
                    StatusCode = 400,
                    Message = "Incorrect username or password, Authentication failed"
                });
            }

            if (!await _appUserService.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError(string.Empty, "User Has not Confirmed Email.");

                return Unauthorized(new { LoginError = "We sent you an Confirmation Email. Please Confirm Your Registration." });
            }

            // username & password matches: create the refresh token
            var newRefreshToken = CreateRefreshToken(user.Id);

            // first we delete any existing old refreshtokens
            var oldRefreshTokens = await _refreshTokenService.GetRangeAsync(user.Id);

            if (oldRefreshTokens != null)
            {
                await _refreshTokenService.DeleteRangeAsync(user.Id);
            }

            // Add new refresh token to Database
            await _refreshTokenService.CreateAsync(newRefreshToken);

            // Create & Return the access token which contains JWT and Refresh Token
            var accessToken = await CreateAccessToken(user, newRefreshToken.Value);

            return Ok(new { accessToken });
        }

        // Create access Token
        private async Task<AccessResponseModel> CreateAccessToken(AppUserDto user, string refreshToken)
        {
            double tokenExpiryTime = Convert.ToDouble(_appSettings.ExpireTime);

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appSettings.Secret));

            var roles = await _appUserService.GetRolesAsync(user);

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Role, roles.FirstOrDefault()),
                        new Claim("LoggedOn", DateTime.Now.ToString()),
                        new Claim("UserID", user.Id.ToString())
                     }),

                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
                Issuer = _appSettings.Site,
                Audience = _appSettings.Audience,
                Expires = DateTime.UtcNow.AddMinutes(tokenExpiryTime)
            };

            // Generate token
            var newtoken = tokenHandler.CreateToken(tokenDescriptor);

            var encodedToken = tokenHandler.WriteToken(newtoken);

            return new AccessResponseModel()
            {
                Token = encodedToken,
                Expiration = newtoken.ValidTo,
                Refresh_token = refreshToken,
                Role = roles.FirstOrDefault(),
                UserName = user.UserName,
                Photo = user.Photo
            };
        }

        // Method to create new RefreshToken
        private RefreshTokenDto CreateRefreshToken(int userId)
        {
            return new RefreshTokenDto()
            {
                Value = Guid.NewGuid().ToString("N"),
                CreatedDate = DateTime.UtcNow,
                UserId = userId,
                ExpiryTime = DateTime.UtcNow.AddMinutes(4)
            };
        }

        // Method to Refresh JWT and Refresh Token
        private async Task<IActionResult> UpdateAccessToken(AccessRequestModel model)
        {
            try
            {
                // check if any user exist in Database with received userName
                var user = await _appUserService.GetByNameAsync(model.UserName);

                if (user == null)
                {
                    // UserId not found or invalid
                    return new UnauthorizedResult();
                }

                // check if the received refreshToken exists for found user
                var refreshToken = await _refreshTokenService.GetByUserIdAsync(user.Id, model.RefreshToken.ToString());

                if (refreshToken == null)
                {
                    // refresh token not found or invalid (or invalid clientId)
                    return new UnauthorizedResult();
                }

                // check if refresh token is expired
                if (refreshToken.ExpiryTime < DateTime.UtcNow)
                {
                    return new UnauthorizedResult();
                }

                // generate a new refresh token 
                var refreshTokenNew = CreateRefreshToken(refreshToken.UserId);

                // invalidate the old refresh token (by deleting it)
                await _refreshTokenService.DeleteAsync(refreshToken.Id);

                // add the new refresh token to Db
                await _refreshTokenService.CreateAsync(refreshTokenNew);

                var accessToken = await CreateAccessToken(user, refreshTokenNew.Value);

                return Ok(new { accessToken });

            }
            catch (Exception)
            {
                return new UnauthorizedResult();
            }
        }
    }
}