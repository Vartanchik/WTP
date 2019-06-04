using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WTP.BLL.DTOs.ServicesDTOs;
using WTP.BLL.Services.Concrete.RefreshTokenService;
using WTP.WebAPI.Utility.Extensions;

namespace WTP.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : Controller
    {
        private readonly IRefreshTokenService _refreshTokenService;

        public TokenController(IRefreshTokenService refreshTokenService)
        {
            _refreshTokenService = refreshTokenService;
        }

        /// <summary>
        /// Create JWT token
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [ProducesResponseType(typeof(AccessResponseDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> GetToken([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto(400, "Invalid value was entered! Please, redisplay form."));
            }

            var verifyResult = await _refreshTokenService.VerifyUser(dto);
            if (!verifyResult.Succeeded) return BadRequest(new ResponseDto(400, "Login faild.", verifyResult.Error));

            var access = await _refreshTokenService.GetAccess(dto.Email);

            return access == null
                ? BadRequest(new ResponseDto(400, "Login faild.", "Something going wrong."))
                : (IActionResult)Ok(new { access, message = "Login successful." });
        }

        /// <summary>
        /// Update JWT token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> UpdateToken(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
                return BadRequest(new ResponseDto(400,
                                                  "Refresh token update faild.",
                                                  "Refresh token is null or empty."));

            var userId = this.GetCurrentUserId();
            var result = await _refreshTokenService
                                    .UpdateAccessToken(new UpdateRefreshTokenDto
                                                                {
                                                                    UserId = userId,
                                                                    RefreshToken = refreshToken
                                                                });

            if (!result.Succeeded) return BadRequest(new ResponseDto(400,
                                                                     "Refresh token update faild.",
                                                                     result.Error));

            var newAccessToken = _refreshTokenService.GetCurrentTokenByUserId(userId);
            return Ok(newAccessToken);
        }
    }
}