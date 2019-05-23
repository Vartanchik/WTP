using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using WTP.BLL.DTOs.AppUserDTOs;
using WTP.BLL.DTOs.AzureDTOs;
using WTP.BLL.DTOs.ServicesDTOs;
using WTP.BLL.Services.AzureBlobStorageService;
using WTP.BLL.Services.Concrete.AppUserService;
using WTP.WebAPI.Utility.Extensions;

namespace WTP.WebAPI.Dto.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IAppUserService _appUserService;
        private readonly IConfiguration _configuration;
        private readonly IAzureBlobStorageService _azureBlobStorageService;

        public UserProfileController(IAppUserService appUserService, IConfiguration configuration, IAzureBlobStorageService azureBlobStorageService)
        {
            _appUserService = appUserService;
            _configuration = configuration;
            _azureBlobStorageService = azureBlobStorageService;
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

            var user = await _appUserService.GetByIdAsync(userId);

            return user == null
                ? NotFound(new ResponseDto(404, "Userprofile not found.", "Something going wrong.")) 
                : (IActionResult)Ok(user);
        }

        /// <summary>
        /// Update current user Userprofile
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Response DTO</returns>
        /// <response code="200">Successful performance</response>
        /// <response code="400">The action failed</response>
        [HttpPut]
        [Authorize(Policy = "RequireLoggedIn")]
        [ProducesResponseType(typeof(ResponseDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> UpdateUserProfile([FromBody] AppUserDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto(400, "User profile updated faild.", "Invalid value was entered! Please, redisplay form."));
            }

            var result = await _appUserService.UpdateAsync(user);

            return result.Succeeded
                ? Ok(new ResponseDto(200,"Completed.", "User profile updated successfully."))
                : (IActionResult)BadRequest(new ResponseDto(400, $"User profile updated faild.", "Something going wrong."));
        }

        /// <summary>
        /// Update current user photo
        /// </summary>
        /// <param name="formData"></param>
        /// <returns>Response DTO (with or without url)</returns>
        /// <response code="200">Successful performance</response>
        /// <response code="400">The action failed</response>
        [HttpPost("[action]")]
        [Authorize(Policy = "RequireLoggedIn")]
        [ProducesResponseType(typeof(ResponseDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> UpdatePhoto([FromForm]PhotoFormDataDto formData)
        {
            var azureStorageConfig = new AzureBlobStorageConfigDto(_configuration["AzureBlobStorage:AccountName"],
                                                                   _configuration["AzureBlobStorage:AccountKey"],
                                                                   _configuration["AzureBlobStorage:ContainerName"],
                                                                   _configuration["Url:ImageStorageUrl"]);

            var fileDataDto = new FileDataDto(formData.File.OpenReadStream(), formData.File.ContentType, formData.File.FileName);

            var userPhotoUrl = await _azureBlobStorageService.UploadFileAsync(fileDataDto, azureStorageConfig);

            var userId = this.GetCurrentUserId();

            var appUserModel = await _appUserService.GetByIdAsync(userId);

            var appUserModelPhoto = appUserModel.Photo;

            if (appUserModelPhoto != null && appUserModelPhoto != _configuration["Photo:DefaultPhoto"])
            {
                await _azureBlobStorageService.DeleteFileIfExistsAsync(appUserModelPhoto, azureStorageConfig);
            }

            appUserModel.Photo = userPhotoUrl;

            await _appUserService.UpdateAsync(appUserModel);

            return (userPhotoUrl != null)
                ? Ok(new ResponseDto(200, "Photo was updated.", userPhotoUrl))
                : (IActionResult)BadRequest(new ResponseDto(400, "Photo update failed."));
        }

        /// <summary>
        /// Get photo by url
        /// </summary>
        /// <returns>FileStreamResult</returns>
        /// <returns>Response DTO</returns>
        /// <response code="200">Returns photo</response>
        /// <response code="404">Photo not found</response>
        [HttpGet("[action]/{photoId:minlength(1)}")]
        [ProducesResponseType(typeof(FileStreamResult), 200)]
        [ProducesResponseType(typeof(ResponseDto), 404)]
        public async Task<IActionResult> Photo()
        {
            var requestUrl = UriHelper.GetDisplayUrl(Request);

            var azureBlobStorageConfigModel = new AzureBlobStorageConfigDto(_configuration["AzureBlobStorage:AccountName"],
                                                                            _configuration["AzureBlobStorage:AccountKey"],
                                                                            _configuration["AzureBlobStorage:ContainerName"],
                                                                            _configuration["Url:ImageStorageUrl"]);

            var fileDataModel =  await _azureBlobStorageService.DownloadFileAsync(requestUrl, azureBlobStorageConfigModel);

            return fileDataModel != null
                ? File(fileDataModel.Stream, fileDataModel.Type, fileDataModel.Name)
                : (IActionResult)BadRequest(new ResponseDto(404, "Photo not found."));
        }
    }
}
