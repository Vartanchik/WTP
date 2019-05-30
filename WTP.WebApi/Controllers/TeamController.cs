using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WTP.BLL.DTOs.AzureDTOs;
using WTP.BLL.DTOs.ServicesDTOs;
using WTP.BLL.DTOs.TeamDTOs;
using WTP.BLL.Services.AzureBlobStorageService;
using WTP.BLL.Services.Concrete.TeamService;
using WTP.WebAPI.Utility.Extensions;
using Microsoft.AspNetCore.Http.Extensions;

namespace WTP.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamController : Controller
    {
        private readonly ITeamService _teamService;
        private readonly IConfiguration _configuration;
        private readonly IAzureBlobStorageService _azureBlobStorageService;

        public TeamController(ITeamService teamService, IConfiguration configuration, IAzureBlobStorageService azureBlobStorageService)
        {
            _teamService = teamService;
            _configuration = configuration;
            _azureBlobStorageService = azureBlobStorageService;
        }

        /// <summary>
        /// Create or update current user team by game id
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [Authorize(Policy = "RequireLoggedIn")]
        [ProducesResponseType(typeof(ResponseDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> PostTeam([FromBody] CreateUpdateTeamDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto(400, "Failed.", "Redisplay form."));
            }

            var userId = this.GetCurrentUserId();
            var result = await _teamService.CreateOrUpdateAsync(dto, userId);

            return result.Succeeded
                ? Ok(new ResponseDto(200, "Completed.", "Player created."))
                : (IActionResult)BadRequest(new ResponseDto(400, "Failed.", result.Error));
        }

        /// <summary>
        /// Delete current user team by game id
        /// </summary>
        /// <param name="teamGameId"></param>
        /// <returns>IActionResult</returns>
        [HttpDelete]
        [Authorize(Policy = "RequireLoggedIn")]
        [ProducesResponseType(typeof(ResponseDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> Delete(int teamGameId)
        {
            var userId = this.GetCurrentUserId();

            var result = await _teamService.DeleteAsync(userId, teamGameId);

            return result.Succeeded
                ? Ok(new ResponseDto(200, "Completed.", "Team deleted."))
                : (IActionResult)BadRequest(new ResponseDto(400, "Failed.", result.Error));
        }

        /// <summary>
        /// Add to team players by id
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        [Authorize(Policy = "RequireLoggedIn")]
        [ProducesResponseType(typeof(ResponseDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> AddPlayer(AddPlayerToTeamDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto(400, "Failed.", "Redisplay form."));
            }

            var userId = this.GetCurrentUserId();
            var result = await _teamService.AddToTeamAsync(dto, userId);

            return result.Succeeded
                ? Ok(new ResponseDto(200, "Completed.", "Players have been added to the team."))
                : (IActionResult)BadRequest(new ResponseDto(400, "Failed.", result.Error));
        }

        /// <summary>
        /// Get all user's teams 
        /// </summary>
        /// <returns>List of teams</returns>
        /// <returns>Response DTO</returns>
        [HttpGet("[action]/{userId:int}")]
        [Authorize(Policy = "RequireLoggedIn")]
        [ProducesResponseType(typeof(IList<TeamListItemDto>), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IList<TeamListItemDto>> ListByUserId(int userId)
        {
            return await _teamService.GetListByUserIdAsync(userId);
        }

        /// <summary>
        /// Update current team logo
        /// </summary>
        /// <param name="formData"></param>
        /// <returns>Response DTO (with or without url)</returns>
        /// <response code="200">Successful performance</response>
        /// <response code="400">The action failed</response>
        [HttpPost("[action]")]
        [Authorize(Policy = "RequireLoggedIn")]
        [ProducesResponseType(typeof(ResponseDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> UpdateLogo([FromForm]PhotoFormDataDto formData, int teamGameId)
        {
            var azureStorageConfig = new AzureBlobStorageConfigDto(_configuration["AzureBlobStorage:AccountKey"],
                                                                    _configuration["AzureBlobStorage:ContainerName"],
                                                                    _configuration["AzureBlobStorage:AccountName"],
                                                                    _configuration["Url:TeamLogoStorageUrl"]);

            var fileDataDto = new FileDataDto(formData.File.OpenReadStream(), formData.File.ContentType, formData.File.FileName);

            var teamLogoUrl = await _azureBlobStorageService.UploadFileAsync(fileDataDto, azureStorageConfig);

            var userId = this.GetCurrentUserId();

            var result = await _teamService.UpdateLogoAsync(userId, teamGameId, teamLogoUrl);

            return (teamLogoUrl != null && result.Succeeded)
                ? Ok(new ResponseDto(200, "Logo was updated.", teamLogoUrl))
                : (IActionResult)BadRequest(new ResponseDto(400, "Logo update failed."));
        }

        /// <summary>
        /// Get logo by url
        /// </summary>
        /// <returns>FileStreamResult</returns>
        /// <returns>Response DTO</returns>
        /// <response code="200">Returns logo</response>
        /// <response code="404">Logo not found</response>
        [HttpGet("[action]/{logoId:minlength(1)}")]
        [ProducesResponseType(typeof(FileStreamResult), 200)]
        [ProducesResponseType(typeof(ResponseDto), 404)]
        public async Task<IActionResult> Logo()
        {
            var requestUrl = UriHelper.GetDisplayUrl(Request);

            var azureBlobStorageConfigModel = new AzureBlobStorageConfigDto(_configuration["AzureBlobStorage:AccountName"],
                                                                            _configuration["AzureBlobStorage:AccountKey"],
                                                                            _configuration["AzureBlobStorage:ContainerName"],
                                                                            _configuration["Url:TeamLogoStorageUrl"]);

            var fileDataModel = await _azureBlobStorageService.DownloadFileAsync(requestUrl, azureBlobStorageConfigModel);

            return fileDataModel != null
                ? File(fileDataModel.Stream, fileDataModel.Type, fileDataModel.Name)
                : (IActionResult)BadRequest(new ResponseDto(404, "Logo not found."));
        }
    }
}
