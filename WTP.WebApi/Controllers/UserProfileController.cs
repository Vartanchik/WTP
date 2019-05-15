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

        //GET : /api/UserProfile/id
        [HttpGet]
        [Authorize(Policy = "RequireLoggedIn")]
        [ProducesResponseType(typeof(AppUserApiDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 404)]
        public async Task<IActionResult> GetUserProfile()
        {
            int userId = this.GetCurrentUserId();

            var appUserDto = await _appUserService.GetAsync(userId);

            var appUserDtoViewModel = _mapper.Map<AppUserApiDto>(appUserDto);

            return appUserDto == null
                ? NotFound(new ResponseDto(404, "Userprofile not found.", "Something going wrong.")) 
                : (IActionResult)Ok(appUserDtoViewModel);
        }

        //PUT : /api/UserProfile
        [HttpPut]
        [Authorize(Policy = "RequireLoggedIn")]
        [ProducesResponseType(typeof(ResponseDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> UpdateUserProfile([FromBody] AppUserApiDto formData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto(400, "User profile updated faild.", "Invalid value was entered! Please, redisplay form."));
            }

            var appUserDto = _mapper.Map<AppUserModel>(formData);

            var result = await _appUserService.UpdateAsync(appUserDto);

            return result.Succeeded
                ? Ok(new ResponseDto(200,"Completed.", "User profile updated successfully."))
                : (IActionResult)BadRequest(new ResponseDto(500, $"User profile updated faild.", "Something going wrong."));
        }
    }
}
