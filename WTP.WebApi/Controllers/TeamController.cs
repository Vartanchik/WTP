using System;
using System.Collections.Generic;
using System.Linq;
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
        /// <param name="gameId"></param>
        /// <returns>IActionResult</returns>
        [HttpDelete]
        [Authorize(Policy = "RequireLoggedIn")]
        [ProducesResponseType(typeof(ResponseDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> Delete(int gameId)
        {
            var userId = this.GetCurrentUserId();

            var result = await _teamService.DeleteAsync(userId, gameId);

            return result.Succeeded
                ? Ok(new ResponseDto(200, "Completed.", "Team deleted."))
                : (IActionResult)BadRequest(new ResponseDto(400, "Failed.", result.Error));
        }

        [HttpPost("[action]")]
        [Authorize(Policy = "RequireLoggedIn")]
        [ProducesResponseType(typeof(ResponseDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> Invite(int playerId)
        {
            var userId = this.GetCurrentUserId();
            // not finished!
            var result = await _teamService.InviteAsync(userId, playerId);

            return result.Succeeded
                ? Ok(new ResponseDto(200, "Completed.", "Players have been invited to the team."))
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
            // not valid logic!
            var result = await _teamService.AddToTeamAsync(dto, userId);

            return result.Succeeded 
                ? Ok(new ResponseDto(200, "Completed.", "Players have been added to the team."))
                : (IActionResult)BadRequest(new ResponseDto(400, "Failed.", result.Error));
        }
    }
}
