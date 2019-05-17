﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WTP.BLL.Services.AzureBlobStorageService;
using WTP.BLL.Services.Concrete.AppUserService;
using WTP.BLL.Models.AppUser;
using WTP.WebAPI.Utility.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Http.Extensions;
using WTP.BLL.Models.Azure;

namespace WTP.WebAPI.Dto.Controllers
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

        [HttpPost("[action]")]
        [Authorize(Policy = "RequireLoggedIn")]
        [ProducesResponseType(typeof(ResponseDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> UpdatePhoto([FromForm]PhotoFormDataDto formData)
        {
            var azureBlobStorageConfigModel = _mapper.Map<AzureBlobStorageConfigModel>(new AzureBlobStorageConfigDto(_configuration));

            var fileDataDto = new FileDataDto(formData.File.OpenReadStream(), formData.File.ContentType, formData.File.FileName);

            var fileDataModel = _mapper.Map<FileDataModel>(fileDataDto);

            var userPhotoUrl = await _azureBlobStorageService.UploadFileAsync(fileDataModel, azureBlobStorageConfigModel);

            var userId = this.GetCurrentUserId();

            var appUserModel = await _appUserService.GetAsync(userId);

            var appUserModelPhoto = appUserModel.Photo;

            if (appUserModelPhoto != null && appUserModelPhoto != _configuration["Photo:DefaultPhoto"])
            {
                await _azureBlobStorageService.DeleteFileIfExistsAsync(appUserModelPhoto, azureBlobStorageConfigModel);
            }

            appUserModel.Photo = userPhotoUrl;

            await _appUserService.UpdateAsync(appUserModel);

            return (userPhotoUrl != null)
                ? Ok(new ResponseDto(200, "Photo was updated.", userPhotoUrl))
                : (IActionResult)BadRequest(new ResponseDto(400, "Photo update failed."));
        }

        [HttpGet("[action]/{photoId:minlength(1)}")]
        [ProducesResponseType(typeof(FileStreamResult), 200)]
        [ProducesResponseType(typeof(ResponseDto), 404)]
        public async Task<IActionResult> Photo()
        {
            var requestUrl = UriHelper.GetDisplayUrl(Request);

            var azureBlobStorageConfigModel = _mapper.Map<AzureBlobStorageConfigModel>(new AzureBlobStorageConfigDto(_configuration));

            var fileDataModel =  await _azureBlobStorageService.DownloadFileAsync(requestUrl, azureBlobStorageConfigModel);

            return fileDataModel != null
                ? File(fileDataModel.Stream, fileDataModel.Type, fileDataModel.Name)
                : (IActionResult)BadRequest(new ResponseDto(404, "Photo not found."));
        }
    }
}
