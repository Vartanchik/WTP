using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WTP.BLL.DTOs.ServicesDTOs;
using WTP.BLL.DTOs.TeamDTOs;
using WTP.BLL.Services.Concrete.InvitationService;
using WTP.WebAPI.Utility.Extensions;

namespace WTP.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class InvitationController : Controller
    {
        IInvitationService _invitationService;

        public InvitationController(IInvitationService invitationService)
        {
            _invitationService = invitationService;
        }

        /// <summary>
        /// Get invitation by id
        /// </summary>
        /// <param name="invitationId"></param>
        [HttpGet("{invitationId}")]
        [Authorize(Policy = "RequireLoggedIn")]
        [ProducesResponseType(typeof(InvitationListItemDto), 200)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> GetInvitationById([FromRoute] int invitationId)
        {
            var invitation = await _invitationService.GetInvitationAsync(invitationId);

            return invitation == null
                ? (IActionResult)NoContent()
                : Ok(invitation);
        }

        /// <summary>
        /// Get invitations by player id
        /// </summary>
        /// <param name="playerId"></param>
        [HttpGet("player/{playerId}")]
        [Authorize(Policy = "RequireLoggedIn")]
        [ProducesResponseType(typeof(List<InvitationListItemDto>), 200)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> GetPlayerInvitationById([FromRoute] int playerId)
        {
            var invitation = await _invitationService.GetPlayerInvitationsAsync(playerId);

            return invitation == null
                ? (IActionResult)NoContent()
                : Ok(invitation);
        }

        /// <summary>
        /// Get all invitations by team id
        /// </summary>
        /// <param name="teamId"></param>
        [HttpGet("team/{teamId}")]
        [Authorize(Policy = "RequireLoggedIn")]
        [ProducesResponseType(typeof(List<InvitationListItemDto>), 200)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> GetTeamInvitationById([FromRoute] int teamId)
        {
            var invitation = await _invitationService.GetTeamInvitationsAsync(teamId);

            return invitation == null
                ? (IActionResult)NoContent()
                : Ok(invitation);
        }

        /// <summary>
        /// Create an invitation to join the team
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="teamId"></param>
        [HttpPost]
        [Authorize(Policy = "RequireLoggedIn")]
        [ProducesResponseType(typeof(ResponseDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> CreateInvitation([FromBody] CreateInvitationDto dto)
        {
            var userId = this.GetCurrentUserId();

            var result = await _invitationService.CreateInvitationAsync(new TeamActionDto
            {
                PlayerId = dto.PlayerId,
                TeamId = dto.TeamId,
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
        [HttpPost("[action]")]
        [Authorize(Policy = "RequireLoggedIn")]
        [ProducesResponseType(typeof(ResponseDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> InvitationResponse([FromBody] InvitationResponseDto dto)
        {
            var userId = this.GetCurrentUserId();

            var invitation = new InviteActionDto
            {
                InvitationId = dto.InvitationId,
                UserId = userId
            };

            var result = dto.Accept
                ? await _invitationService.AcceptInvitationAsync(invitation)
                : await _invitationService.DeclineInvitationAsync(invitation);

            return result.Succeeded
                ? Ok(new ResponseDto(200, "Completed.", "Invite accept."))
                : (IActionResult)BadRequest(new ResponseDto(400, "Failed.", result.Error));
        }
    }
}
