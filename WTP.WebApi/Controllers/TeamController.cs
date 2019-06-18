using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using WTP.BLL.DTOs.AzureDTOs;
using WTP.BLL.DTOs.ServicesDTOs;
using WTP.BLL.DTOs.TeamDTOs;
using WTP.BLL.Services.AzureBlobStorageService;
using WTP.BLL.Services.Concrete.TeamService;
using WTP.WebAPI.Utility.Extensions;

namespace WTP.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
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
        /// Get team DTO by team Id
        /// </summary>
        /// <param name="teamId"></param>
        [HttpGet("[action]/{teamId}")]
        [ProducesResponseType(typeof(TeamDto), 200)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Get([FromRoute] int teamId)
        {
            var team = await _teamService.GetTeamAsync(teamId);

            return team == null ? NoContent() : (IActionResult)Ok(team);
        }

        /// <summary>
        /// Create team for current user
        /// </summary>
        /// <param name="dto"></param>
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
        /// Update team of current user
        /// </summary>
        /// <param name="dto"></param>
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
        /// Delete curren user team by Id
        /// </summary>
        /// <param name="teamId"></param>
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
        /// Create an invitation to join the team
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="teamId"></param>
        [HttpPost("[action]")]
        [Authorize(Policy = "RequireLoggedIn")]
        [ProducesResponseType(typeof(ResponseDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> Invite(int playerId, int teamId)
        {
            var userId = this.GetCurrentUserId();
            var result = await _teamService.CreateInvitationAsync(new TeamActionDto
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
        /// Accept invitations to join the team
        /// </summary>
        /// <param name="dto"></param>
        [HttpPut("[action]")]
        [Authorize(Policy = "RequireLoggedIn")]
        [ProducesResponseType(typeof(ResponseDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> AcceptInvitation(InvitationResponseDto dto)
        {
            var userId = this.GetCurrentUserId();

            var invitation = new InviteActionDto
            {
                InvitationId = dto.InvitationId,
                UserId = userId
            };

            var result = dto.Accept 
                ? await _teamService.AcceptInvitationAsync(invitation)
                : await _teamService.DeclineInvitationAsync(invitation);

            return result.Succeeded
                ? Ok(new ResponseDto(200, "Completed.", "Invite accept."))
                : (IActionResult)BadRequest(new ResponseDto(400, "Failed.", result.Error));
        }

        /// <summary>
        /// Remove player from team
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="teamId"></param>
        [HttpDelete("[action]")]
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
        /// Update team logo
        /// </summary>
        /// <param name="formData"></param>
        /// <param name="teamId"></param>
        [HttpPost("[action]")]
        [Authorize(Policy = "RequireLoggedIn")]
        [ProducesResponseType(typeof(ResponseDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> UpdateLogo([FromForm] PhotoFormDataDto formData, int teamId)
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
        /// Get team logo
        /// </summary>
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
        /// Get all invitations associated with the user
        /// </summary>
        /// <param name="userId"></param>
        [HttpGet("[action]")]
        [ProducesResponseType(typeof(IList<InvitationListItemDto>), 200)]
        public async Task<IList<InvitationListItemDto>> InvitationTeamListByUserId(int userId)
        {
            return await _teamService.GetAllTeamInvitetionByUserId(userId);
        }

        /// <summary>
        /// Get all user teams
        /// </summary>
        /// <param name="userId"></param>
        [HttpGet("[action]")]
        [ProducesResponseType(typeof(IList<TeamListItemDto>), 200)]
        public async Task<IList<TeamListItemDto>> ListByUserId(int userId)
        {
            return await _teamService.GetListByUserIdAsync(userId);
        }

        //Get List of all players by game with filers and sorting
        [HttpGet("teams/pagination")]
        public async Task<TeamIndexDto> TeamIndex([FromQuery] TeamControllerInputDto valuesFromUi)
        {
            TeamInputValuesModelDto inputValues = new TeamInputValuesModelDto()
            {
                GameId = valuesFromUi.IdGame,
                Page = valuesFromUi.Page,
                PageSize = valuesFromUi.PageSize,
                SortField = valuesFromUi.SortField,
                SortType = valuesFromUi.SortType,
                NameValue = valuesFromUi.NameValue,
                WinRateLeftValue = valuesFromUi.WinRateLeftValue,
                WinRateRightValue = valuesFromUi.WinRateRightValue,
                MembersLeftValue = valuesFromUi.MembersLeftValue,
                MembersRightValue = valuesFromUi.MembersRightValue,
                GoalDtos = valuesFromUi.GoalDtos
            };

            TeamPaginationDto inputModel = await _teamService.GetFilteredTeamsByGameIdAsync(inputValues);

            List<TeamListItemDto> teams = new List<TeamListItemDto>(inputModel.Teams);
            if (teams == null)
                return null;


            // representation model
            TeamIndexDto viewModel = new TeamIndexDto
            {
                PageViewModel = new PageDto(inputModel.TeamsQuantity, valuesFromUi.Page, valuesFromUi.PageSize),
                Teams = teams
            };

            return viewModel;
        }


    }
}
