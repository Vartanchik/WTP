﻿using System;
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

        private readonly IAppUserService _appUserService;

        public PlayerController(IPlayerService playerService, IAppUserService appUserService)
        {
            _playerService = playerService;
            _appUserService = appUserService;
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
        /// <returns>Response DTO</returns>
        /// <response code="200">Returns list of players</response>
        /// <response code="400">Get players failed</response>
        [HttpGet("[action]/{userId:int}")]
        [Authorize(Policy = "RequireLoggedIn")]
        [ProducesResponseType(typeof(IList<PlayerListItemDto>), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IList<PlayerListItemDto>> GetPlayersOfUser(int userId)
        {
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

        //Get List of all players
        [HttpGet]
        [Route("players")]
        //[Authorize(Policy = "RequireAdministratorRole")]
        public async Task<PlayerListItemDto[]> GetPlayersProfilesByGame(int idGame)
        {
            var players = await _playerService.GetListByGameIdAsync(idGame);

            if (players == null)
                return null;

            return players.ToArray();
        }

    }
}