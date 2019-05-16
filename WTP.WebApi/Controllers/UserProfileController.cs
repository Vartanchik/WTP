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
using Microsoft.AspNetCore.Http.Extensions;
using WTP.BLL.ModelsDto.Azure;

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
                ? Ok(new ResponseViewModel(200, "Completed.", "User profile updated successfully."))
                : (IActionResult)BadRequest(new ResponseViewModel(500, $"User profile updated faild.", "Something going wrong."));
        }

        [HttpPost("[action]")]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> UploadPhoto([FromForm]PhotoFormDataModel formData)
        {
            var azureBlobStorageConfigDto = _mapper.Map<AzureBlobStorageConfigDto>(new AzureBlobStorageConfigModel(_configuration));

            var fileDataModel = new FileDataModel(formData.File.OpenReadStream(), formData.File.ContentType, formData.File.FileName);

            var fileDataDto = _mapper.Map<FileDataDto>(fileDataModel);

            string userPhotoUrl = await _azureBlobStorageService.UploadFileAsync(fileDataDto, azureBlobStorageConfigDto);

            int userId = this.GetCurrentUserId();

            var result = await _appUserService.UpdatePhotoAsync(userId, userPhotoUrl);

            return (userPhotoUrl != null && result.Succeeded)
                ? Ok(new ResponseViewModel(200, "Photo was updated.", userPhotoUrl))
                : (IActionResult)BadRequest(new ResponseViewModel(400, "Photo updated failed."));
        }

        [HttpGet("[action]/{imageId:minlength(1)}")]
        public async Task<IActionResult> Image()
        {
            var requestUrl = UriHelper.GetDisplayUrl(Request);

            var azureBlobStorageConfigDto = _mapper.Map<AzureBlobStorageConfigDto>(new AzureBlobStorageConfigModel(_configuration));

            var fileDataDto =  await _azureBlobStorageService.DownloadFileAsync(requestUrl, azureBlobStorageConfigDto);

            return File(fileDataDto.Stream, fileDataDto.Type, fileDataDto.Name);
        }
    }
}
