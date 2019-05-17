using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WTP.BLL.Services.Concrete.AppUserService;
using WTP.BLL.Models.AppUser;
using WTP.WebAPI.Utility.Extensions;
using AutoMapper;

namespace WTP.WebAPI.Dto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IAppUserService _appUserService;
        private readonly IMapper _mapper;

        public UserProfileController(IAppUserService appUserService, IMapper mapper)
        {
            _appUserService = appUserService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get Userprofile of current user
        /// </summary>
        /// <returns>Current Userprofile</returns>
        /// <returns>Response DTO</returns>
        /// <response code="200">Returns AppUser DTO</response>
        /// <response code="404">Userprofile not found</response>
        [HttpGet]
        [Authorize(Policy = "RequireLoggedIn")]
        [ProducesResponseType(typeof(AppUserDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 404)]
        public async Task<IActionResult> GetUserProfile()
        {
            int userId = this.GetCurrentUserId();

            var appUserModel = await _appUserService.GetAsync(userId);

            var appUserDto = _mapper.Map<AppUserDto>(appUserModel);

            return appUserDto == null
                ? NotFound(new ResponseDto(404, "Userprofile not found.", "Something going wrong.")) 
                : (IActionResult)Ok(appUserDto);
        }

        /// <summary>
        /// Update current user Userprofile
        /// </summary>
        /// <param name="formData"></param>
        /// <returns>Response DTO</returns>
        /// <response code="200">Successful performance</response>
        /// <response code="400">The action failed</response>
        [HttpPut]
        [Authorize(Policy = "RequireLoggedIn")]
        [ProducesResponseType(typeof(ResponseDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> UpdateUserProfile([FromBody] AppUserDto formData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto(400, "User profile updated faild.", "Invalid value was entered! Please, redisplay form."));
            }

            var appUserModel = _mapper.Map<AppUserModel>(formData);

            var result = await _appUserService.UpdateAsync(appUserModel);

            return result.Succeeded
                ? Ok(new ResponseDto(200,"Completed.", "User profile updated successfully."))
                : (IActionResult)BadRequest(new ResponseDto(400, $"User profile updated faild.", "Something going wrong."));
        }
    }
}
