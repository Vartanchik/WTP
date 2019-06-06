using System.Collections.Generic;
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
        /// Get team
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ResponseDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<TeamDto> Get(int teamId)
        {
            return await _teamService.GetTeamAsync(teamId);
        }

        /// <summary>
        /// Get team id
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        [ProducesResponseType(typeof(int), 200)]
        public int GetTeamIdByGameId(int gameId)
        {
            var userId = this.GetCurrentUserId();

            return _teamService.GetTeamIdByGameId(userId, gameId);
        }

        /// <summary>
        /// Get team players quantity
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns>Quantity of players or -1 if this game's team isn't exist</returns>
        [HttpGet("[action]")]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<int> GetTeamPlayersQuantityByGame(int gameId)
        {
            var userId = this.GetCurrentUserId();

            return await _teamService.GetPlayersQuantityAsync(userId, gameId);
        }

        /// <summary>
        /// Create team
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "RequireLoggedIn")]
        [ProducesResponseType(typeof(ResponseDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> Create(CreateTeamDto dto)
        {
            var userId = this.GetCurrentUserId();
            var result = await _teamService.CreateAsync(dto, userId);

            return result.Succeeded
                ? Ok(new ResponseDto(200, "Completed.", "Team created."))
                : (IActionResult)BadRequest(new ResponseDto(400, "Failed.", result.Error));
        }

        /// <summary>
        /// Update team
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Policy = "RequireLoggedIn")]
        [ProducesResponseType(typeof(ResponseDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> Update(UpdateTeamDto dto)
        {
            var userId = this.GetCurrentUserId();
            var result = await _teamService.UpdateAsync(dto, userId);

            return result.Succeeded
                ? Ok(new ResponseDto(200, "Completed.", "Team updated."))
                : (IActionResult)BadRequest(new ResponseDto(400, "Failed.", result.Error));
        }

        /// <summary>
        /// Delete current user team by game id
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns>IActionResult</returns>
        [HttpDelete]
        [Authorize(Policy = "RequireLoggedIn")]
        [ProducesResponseType(typeof(ResponseDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> Delete(int teamId)
        {
            var userId = this.GetCurrentUserId();
            var result = await _teamService.DeleteAsync(teamId, userId);

            return result.Succeeded
                ? Ok(new ResponseDto(200, "Completed.", "Team deleted."))
                : (IActionResult)BadRequest(new ResponseDto(400, "Failed.", result.Error));
        }

        /// <summary>
        /// Invite player to team
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="teamId"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [Authorize(Policy = "RequireLoggedIn")]
        [ProducesResponseType(typeof(ResponseDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> InvitePlayer(int playerId, int teamId)
        {
            var userId = this.GetCurrentUserId();
            var result = await _teamService.InviteToPlayerAsync(new TeamActionDto
            {
                PlayerId = playerId,
                TeamId = teamId,
                UserId = userId
            });

            return result.Succeeded
                ? Ok(new ResponseDto(200, "Completed.", "Invite created."))
                : (IActionResult)BadRequest(new ResponseDto(400, "Failed.", result.Error));
        }

        /// <summary>
        /// Remove player from team
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="teamId"></param>
        /// <returns>IActionResult</returns>
        [HttpPut("[action]")]
        [Authorize(Policy = "RequireLoggedIn")]
        [ProducesResponseType(typeof(ResponseDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> RemovePlayerFromTeam(int playerId, int teamId)
        {
            var userId = this.GetCurrentUserId();

            var result = await _teamService.RemoveFromTeamAsync(new TeamActionDto
            {
                PlayerId = playerId,
                TeamId = teamId,
                UserId = userId
            });

            return result.Succeeded
                ? Ok(new ResponseDto(200, "Completed.", "Player removed from team."))
                : (IActionResult)BadRequest(new ResponseDto(400, "Failed.", result.Error));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="teamId"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [Authorize(Policy = "RequireLoggedIn")]
        [ProducesResponseType(typeof(ResponseDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> InviteTeam(int playerId, int teamId)
        {
            var userId = this.GetCurrentUserId();
            var result = await _teamService.InviteToTeamAsync(new TeamActionDto
            {
                PlayerId = playerId,
                TeamId = teamId,
                UserId = userId
            });

            return result.Succeeded
                ? Ok(new ResponseDto(200, "Completed.", "Invite created."))
                : (IActionResult)BadRequest(new ResponseDto(400, "Failed.", result.Error));
        }

        /// Get list of teams
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>IList<TeamListItemDto></returns>
        [HttpGet("[action]")]
        [ProducesResponseType(typeof(IList<TeamListItemDto>), 200)]
        public async Task<IList<TeamListItemDto>> ListByUserId(int userId)
        {
            return await _teamService.GetListByUserIdAsync(userId);
        }

        /// <summary>
        /// Update current team logo
        /// </summary>
        /// <param name="formData"></param>
        /// <returns>Response DTO (with or without url)</returns>
        [HttpPost("[action]")]
        [Authorize(Policy = "RequireLoggedIn")]
        [ProducesResponseType(typeof(ResponseDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> AcceptInvitation(int invitationId)
        {
            var userId = this.GetCurrentUserId();
            var result = await _teamService.AcceptInvitationAsync(new InviteActionDto
            {
                InviteId = invitationId,
                UserId = userId
            });

            return result.Succeeded
                ? Ok(new ResponseDto(200, "Completed.", "Invite accept."))
                : (IActionResult)BadRequest(new ResponseDto(400, "Failed.", result.Error));
        }

        [HttpPost("[action]")]
        [Authorize(Policy = "RequireLoggedIn")]
        [ProducesResponseType(typeof(ResponseDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> DeclineInvitation(int invitationId)
        {
            var userId = this.GetCurrentUserId();
            var result = await _teamService.DeclineInvitationAsync(new InviteActionDto
            {
                InviteId = invitationId,
                UserId = userId
            });

            return result.Succeeded
                ? Ok(new ResponseDto(200, "Completed.", "Invite decline."))
                : (IActionResult)BadRequest(new ResponseDto(400, "Failed.", result.Error));
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
        public async Task<IActionResult> UpdateLogo([FromForm]PhotoFormDataDto formData, int teamId)
        {
            var azureStorageConfig = new AzureBlobStorageConfigDto(_configuration["AzureBlobStorage:AccountName"],
                                                                   _configuration["AzureBlobStorage:AccountKey"],
                                                                   _configuration["AzureBlobStorage:ContainerName"],
                                                                   _configuration["Url:TeamLogoStorageUrl"]);

            var fileDataDto = new FileDataDto(formData.File.OpenReadStream(), formData.File.ContentType, formData.File.FileName);

            var teamLogoUrl = await _azureBlobStorageService.UploadFileAsync(fileDataDto, azureStorageConfig);

            var userId = this.GetCurrentUserId();

            var result = await _teamService.UpdateLogoAsync(userId, teamId, teamLogoUrl);

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

        /// <summary>
        /// Get list of team's invitations
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        [Authorize(Policy = "RequireLoggedIn")]
        [ProducesResponseType(typeof(IList<InvitationListItemDto>), 200)]
        public async Task<IList<InvitationListItemDto>> InvitationTeamListByUserId()
        {
            var userId = this.GetCurrentUserId();

            return await _teamService.GetAllTeamInvitetionByUserId(userId);
        }
    }
}
