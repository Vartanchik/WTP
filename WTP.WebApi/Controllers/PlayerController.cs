using System;
using System.Collections.Generic;
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
    [Route("api/Player")]
    [ApiController]
    public class PlayerController : Controller
    {
        private readonly IPlayerService _playerService;

        private readonly IAppUserService _appUserService;

        public PlayerController(IPlayerService playerService, IAppUserService appUserService)
        {
            _playerService = playerService;
            _appUserService = appUserService;
        }

        /// <summary>
        /// Get all user's players 
        /// </summary>
        /// <returns>List of players</returns>
        /// <returns>Response DTO</returns>
        /// <response code="200">Returns list of players</response>
        /// <response code="400">Get players failed</response>
        [HttpGet("[action]")]
        [Authorize(Policy = "RequireLoggedIn")]
        [ProducesResponseType(typeof(IList<PlayerDto>), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IList<PlayerListItemDto>> GetPlayersOfUser()
        {
            int userId = this.GetCurrentUserId();

            return await _playerService.GetListByUserIdAsync(userId);
        }

        [HttpGet("[action]")]
        [Authorize(Policy = "RequireLoggedIn")]
        [ProducesResponseType(typeof(IList<PlayerDto>), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> DeletePlayer(int playerId)
        {
            int userId = this.GetCurrentUserId();

            await _playerService.DeleteAsync(userId, playerId);

            return Ok();
        }
    }
}