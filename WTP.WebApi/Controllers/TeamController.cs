using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WTP.BLL.DTOs.ServicesDTOs;
using WTP.BLL.DTOs.TeamDTOs;
using WTP.BLL.Services.Concrete.TeamService;
using WTP.WebAPI.Utility.Extensions;

namespace WTP.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamController : Controller
    {
        private readonly ITeamService _teamService;

        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
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
        /// Create team
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "RequireLoggedIn")]
        [ProducesResponseType(typeof(ResponseDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> Create(CreateOrUpdateTeamDto dto)
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
        public async Task<IActionResult> Update(CreateOrUpdateTeamDto dto)
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
        /// 
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
                ? Ok(new ResponseDto(200, "Completed.", "Invite accept."))
                : (IActionResult)BadRequest(new ResponseDto(400, "Failed.", result.Error));
        }
    }
}
