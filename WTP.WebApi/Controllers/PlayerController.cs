using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WTP.BLL.DTOs.PlayerDTOs;
using WTP.BLL.DTOs.ServicesDTOs;
using WTP.BLL.Services.Concrete.AppUserService;
using WTP.BLL.Services.Concrete.PlayerSrvice;
using WTP.WebAPI.Utility.Extensions;

namespace WTP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : Controller
    {
        private readonly IPlayerService _playerService;

        public PlayerController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        /// <summary>
        /// Create new player
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>Response DTO</returns>
        [HttpPost]
        [Authorize(Policy = "RequireLoggedIn")]
        [ProducesResponseType(typeof(ResponseDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> PostUser([FromBody] CreateUpdatePlayerDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto(400, "Failed.", "Redisplay form."));
            }

            var userId = this.GetCurrentUserId();
            var result = await _playerService.CreateOrUpdateAsync(dto, userId);

            return result.Succeeded
                ? Ok(new ResponseDto(200, "Completed.", "Player created."))
                : (IActionResult)BadRequest(new ResponseDto(400, "Failed.", result.Error));
        }

        /// <summary>
        /// Delete player
        /// </summary>
        /// <param name="playerGameId"></param>
        [HttpDelete]
        [Authorize(Policy = "RequireLoggedIn")]
        [ProducesResponseType(typeof(ResponseDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> Delete(int playerGameId)
        {
            var userId = this.GetCurrentUserId();

            var result = await _playerService.DeleteAsync(userId, playerGameId);

            return result.Succeeded
                ? Ok(new ResponseDto(200, "Completed.", "Player deleted."))
                : (IActionResult)BadRequest(new ResponseDto(400, "Failed.", result.Error));
        }

        /// <summary>
        /// Get all user's players 
        /// </summary>
        /// <returns>List of players</returns>
        /// <returns>List<PlayerListItemDto></returns>
        [HttpGet("[action]")]
        [ProducesResponseType(typeof(IList<PlayerListItemDto>), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IList<PlayerListItemDto>> GetPlayersByUser(int userId)
        {
            return await _playerService.GetListByUserIdAsync(userId);
        }

        /// <summary>
        /// Get list players by team
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns>List<PlayerListItemDto></returns>
        [HttpGet("[action]")]
        [ProducesResponseType(typeof(IList<PlayerListItemDto>), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IList<PlayerListItemDto>> GetPlayersByTeam(int teamId)
        {
            return await _playerService.GetListByTeamIdAsync(teamId);
        }

        //Get List of all players by game
        [HttpGet("players/pagination")]
        public async Task<PlayerIndexDto> PlayerIndex(int idGame, int page = 1)
        {
            int pageSize = 5;

            List<PlayerListItemDto> players = new List<PlayerListItemDto>(await _playerService.GetListByGameIdAsync(idGame));
            if (players == null)
                return null;

            // Pagination
            var count = players.Count();
            var items = players.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // representation model
            PlayerIndexDto viewModel = new PlayerIndexDto
            {
                PageViewModel = new PageDto(count, page, pageSize),
                Players = items
            };

            return viewModel;
        }
    }
}