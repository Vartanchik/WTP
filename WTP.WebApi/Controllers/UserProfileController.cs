using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WTP.BLL.Services.Concrete.AzureBlobStorageService;
using WTP.BLL.Services.Concrete.AppUserService;
using WTP.BLL.ModelsDto.AppUser;
using WTP.WebAPI.Utility.Extensions;
using AutoMapper;

namespace WTP.WebAPI.ViewModels.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IAppUserService _appUserService;

        private readonly IMapper _mapper;

        private readonly IConfiguration _configuration;

        private readonly IAzureBlobStorageService _azureBlobStorageService;

        public UserProfileController(IAppUserService appUserService, IMapper mapper, IConfiguration configuration, IAzureBlobStorageService azureBlobStorageService)
        {
            _appUserService = appUserService;

            _mapper = mapper;

            _configuration = configuration;

            _azureBlobStorageService = azureBlobStorageService;
        }

        //GET : /api/UserProfile
        [HttpGet]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> GetUserProfile()
        {
            int userId = this.GetCurrentUserId();
            var appUserDto = await _appUserService.GetAsync(userId);

            var appUserDtoViewModel = _mapper.Map<AppUserDtoViewModel>(appUserDto);

            return appUserDto == null
                ? NotFound(new ResponseViewModel(404, "User not found.", "Something going wrong.")) 
                : (IActionResult)Ok(appUserDtoViewModel);
        }

        //PUT : /api/UserProfile
        [HttpPut]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> UpdateUserProfile([FromBody] AppUserDtoViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseViewModel(400, "User profile updated faild.", "Invalid value was entered! Please, redisplay form."));
            }

            var appUserDto = _mapper.Map<AppUserDto>(formData);

            var result = await _appUserService.UpdateAsync(appUserDto);

            return result.Succeeded
                ? Ok(new ResponseViewModel(200,"Completed.", "User profile updated successfully."))
                : (IActionResult)BadRequest(new ResponseViewModel(500, $"User profile updated faild.", "Something going wrong."));
        }

        [HttpPost("[action]")]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> UploadFile([FromForm]FileFormDataModel formData)
        {
            int userId = this.GetCurrentUserId();

            var user = await _appUserService.GetAsync(userId);

            var userPhoto = await _azureBlobStorageService.UploadFileAsync(formData.File, user.Photo);

            if (user.Photo == null)
            {
                user.Photo = userPhoto;

                var result = await _appUserService.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    return BadRequest(new ResponseViewModel(400, "Photo updated failed."));
                }
            }

            var photoUrl = _configuration["Url:FileStorageUrl"] + userPhoto;

            return Ok(new ResponseViewModel(200, "User photo was updated.", photoUrl));
        }

        [HttpGet("[action]")]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> Files([FromQuery]string file)
        {
            return await _azureBlobStorageService.DownloadFileAsync(file);
        }
    }
}
