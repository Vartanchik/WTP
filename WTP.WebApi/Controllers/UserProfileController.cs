using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WTP.BLL.Services.AzureBlobStorageService;
using WTP.BLL.Services.Concrete.AppUserService;
using WTP.BLL.Dto.AppUser;
using WTP.WebAPI.Utility.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Http.Extensions;
using WTP.BLL.Dto.Azure;

namespace WTP.WebAPI.Models.Controllers
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

        /// <summary>
        /// Get Userprofile of current user
        /// </summary>
        /// <returns>Current Userprofile</returns>
        /// <returns>ResponseModel</returns>
        /// <response code="200">Returns AppUserDto</response>
        /// <response code="404">Userprofile not found</response>
        [HttpGet]
        [Authorize(Policy = "RequireLoggedIn")]
        [ProducesResponseType(typeof(AppUserDto), 200)]
        [ProducesResponseType(typeof(ResponseModel), 404)]
        public async Task<IActionResult> GetUserProfile()
        {
            int userId = this.GetCurrentUserId();

            var appUserDto = await _appUserService.GetAsync(userId);

            return appUserDto == null
                ? NotFound(new ResponseModel(404, "Userprofile not found.", "Something going wrong.")) 
                : (IActionResult)Ok(appUserDto);
        }

        /// <summary>
        /// Update current user Userprofile
        /// </summary>
        /// <param name="formData"></param>
        /// <returns>ResponseModel</returns>
        /// <response code="200">Successful performance</response>
        /// <response code="400">The action failed</response>
        [HttpPut]
        [Authorize(Policy = "RequireLoggedIn")]
        [ProducesResponseType(typeof(ResponseModel), 200)]
        [ProducesResponseType(typeof(ResponseModel), 400)]
        public async Task<IActionResult> UpdateUserProfile([FromBody] AppUserDto formData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseModel(400, "User profile updated faild.", "Invalid value was entered! Please, redisplay form."));
            }

            var result = await _appUserService.UpdateAsync(formData);

            return result.Succeeded
                ? Ok(new ResponseModel(200,"Completed.", "User profile updated successfully."))
                : (IActionResult)BadRequest(new ResponseModel(400, $"User profile updated faild.", "Something going wrong."));
        }

        /// <summary>
        /// Update current user photo
        /// </summary>
        /// <param name="formData"></param>
        /// <returns>ResponseModel (with or without url)</returns>
        /// <response code="200">Successful performance</response>
        /// <response code="400">The action failed</response>
        [HttpPost("[action]")]
        [Authorize(Policy = "RequireLoggedIn")]
        [ProducesResponseType(typeof(ResponseModel), 200)]
        [ProducesResponseType(typeof(ResponseModel), 400)]
        public async Task<IActionResult> UpdatePhoto([FromForm]PhotoFormDataModel formData)
        {
            var azureBlobStorageConfigDto = _mapper.Map<AzureBlobStorageConfigDto>(new AzureBlobStorageConfigModel(_configuration));

            var fileDataDto = new FileDataDto(formData.File.OpenReadStream(), formData.File.ContentType, formData.File.FileName);

            var userPhotoUrl = await _azureBlobStorageService.UploadFileAsync(fileDataDto, azureBlobStorageConfigDto);

            var userId = this.GetCurrentUserId();

            var appUserDto = await _appUserService.GetAsync(userId);

            var appUserDtoPhoto = appUserDto.Photo;

            if (appUserDtoPhoto != null && appUserDtoPhoto != _configuration["Photo:DefaultPhoto"])
            {
                await _azureBlobStorageService.DeleteFileIfExistsAsync(appUserDtoPhoto, azureBlobStorageConfigDto);
            }

            appUserDto.Photo = userPhotoUrl;

            await _appUserService.UpdateAsync(appUserDto);

            return (userPhotoUrl != null)
                ? Ok(new ResponseModel(200, "Photo was updated.", userPhotoUrl))
                : (IActionResult)BadRequest(new ResponseModel(400, "Photo update failed."));
        }

        /// <summary>
        /// Get photo by url
        /// </summary>
        /// <returns>FileStreamResult</returns>
        /// <returns>ResponseModel</returns>
        /// <response code="200">Returns photo</response>
        /// <response code="404">Photo not found</response>
        [HttpGet("[action]/{photoId:minlength(1)}")]
        [ProducesResponseType(typeof(FileStreamResult), 200)]
        [ProducesResponseType(typeof(ResponseModel), 404)]
        public async Task<IActionResult> Photo()
        {
            var requestUrl = UriHelper.GetDisplayUrl(Request);

            var azureBlobStorageConfigDto = _mapper.Map<AzureBlobStorageConfigDto>(new AzureBlobStorageConfigModel(_configuration));

            var fileDataDto =  await _azureBlobStorageService.DownloadFileAsync(requestUrl, azureBlobStorageConfigDto);

            return fileDataDto != null
                ? File(fileDataDto.Stream, fileDataDto.Type, fileDataDto.Name)
                : (IActionResult)BadRequest(new ResponseModel(404, "Photo not found."));
        }
    }
}
