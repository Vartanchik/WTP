using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WTP.BLL.DTOs.AppUserDTOs;
using WTP.BLL.DTOs.ServicesDTOs;
using WTP.BLL.Services.Concrete.AppUserService;
using WTP.BLL.Services.Concrete.RefreshTokenService;
using WTP.WebAPI.Helpers;

namespace WTP.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : Controller
    {
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

        /// <summary>
        /// Authentication and authorization
        /// </summary>
        /// <param name="model"></param>
        /// <returns>AccessRequest DTO</returns>
        /// <returns>Response DTO</returns>
        /// <response code="200">Login or Token update</response>
        /// <response code="401">Unauthorized rsult</response>
        [HttpPost("[action]")]
        [ProducesResponseType(typeof(AccessResponseDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> Auth([FromBody] AccessRequestDto model) // granttype = "refresh_token or password"
        {
            if (model == null)
            {
                return BadRequest(new ResponseDto(500, "Login failed.", "Something going wrong."));
            }

            switch (model.GrantType)
            {
                case "password":
                    return await Login(model);
                case "refresh_token":
                    return await UpdateAccessToken(model);
                default:
                    return Unauthorized(new ResponseDto(401,
                                                        "Login failed.",
                                                        "Something going wrong."));
            }
        }

        private async Task<IActionResult> Login(AccessRequestDto model)
        {
            var user = await _appUserService.GetByEmailAsync(model.Email);

            if (user != null && await _appUserService.CheckPasswordAsync(user.Id, model.Password))
            {
                if (!await _appUserService.IsEmailConfirmedAsync(user.Id))
                {
                    return BadRequest(new ResponseDto(400,
                                                        "Login failed.",
                                                        "We sent you an confirmation email. Please confirm your registration."));
                }

                if (user.IsDeleted)
                {
                    return BadRequest(new ResponseDto(400,
                                                        "Login failed.",
                                                        "Your account has been deleted. We have sent an email to reset your account"));
                }

                var newRefreshToken = CreateRefreshToken(user.Id);

                var oldRefreshTokens = _refreshTokenService.GetRangeAsync(user.Id);

                if (oldRefreshTokens != null)
                {
                    await _refreshTokenService.DeleteRangeAsync(user.Id);
                }

                await _refreshTokenService.CreateAsync(newRefreshToken);

                var accessToken = await CreateAccessToken(user, newRefreshToken.Value);

                return Ok(new { accessToken, message = "Login successful." });
            }

            return BadRequest(new ResponseDto(400,
                                              "Authentication failed.",
                                              "Incorrect email or password."));
        }

        private async Task<AccessResponseDto> CreateAccessToken(BLL.DTOs.AppUserDTOs.AppUserDto user, string refreshToken)
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

            var newtoken = tokenHandler.CreateToken(tokenDescriptor);

            var encodedToken = tokenHandler.WriteToken(newtoken);

            return new AccessResponseDto()
            {
                Token = encodedToken,
                Expiration = newtoken.ValidTo,
                Refresh_token = refreshToken,
                Role = roles.FirstOrDefault(),
                UserName = user.UserName,
                Photo = user.Photo
            };
        }

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

        private async Task<IActionResult> UpdateAccessToken(AccessRequestDto model)
        {
            try
            {
                var user = await _appUserService.GetByNameAsync(model.UserName);

                if (user == null)
                {
                    return new UnauthorizedResult();
                }

                var refreshToken = await _refreshTokenService.GetByUserIdAsync(user.Id, model.RefreshToken.ToString());

                if (refreshToken == null)
                {
                    return new UnauthorizedResult();
                }

                // check if refresh token is expired
                if (refreshToken.ExpiryTime < DateTime.UtcNow)
                {
                    return new UnauthorizedResult();
                }

                var refreshTokenNew = CreateRefreshToken(refreshToken.UserId);

                await _refreshTokenService.DeleteAsync(refreshToken.Id);

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