using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WTP.BLL.DTOs.ServicesDTOs;
using WTP.BLL.DTOs.TokensDTOs;
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
        /// Get access and refresh tokens
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [ProducesResponseType(typeof(AccessDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> GetAccess([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto(400, "Invalid value was entered! Please, redisplay form."));
            }

            // user verification
            var verify = await _refreshTokenService.UserVerifyAsync(dto);
            if (!verify.Succeeded) return BadRequest(new ResponseDto(400, "Login faild.", verify.Error));

            var access = await _refreshTokenService.GetAccessAsync(dto.Email);

            return access == null
                ? BadRequest(new ResponseDto(400, "Login faild.", "Something going wrong."))
                : (IActionResult)Ok(access);
        }

        /// <summary>
        /// Update access and refresh tokens
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [Authorize(Policy = "RequireLoggedIn")]
        [ProducesResponseType(typeof(AccessDto), 200)]
        public async Task<IActionResult> RefreshAccess([FromBody] string refreshToken)
        {
            var userId = this.GetCurrentUserId();

            var newAccess = await _refreshTokenService.UpdateAccessAsync(
                new AccessOperationDto
                {
                    UserId = userId,
                    RefreshToken = refreshToken
                });

            return Ok(newAccess);
        }
    }
}
