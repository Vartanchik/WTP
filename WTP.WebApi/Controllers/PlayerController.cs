using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WTP.BLL.DTOs.PlayerDTOs;
using WTP.BLL.DTOs.ServicesDTOs;
using WTP.BLL.DTOs.TeamDTOs;
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
        public async Task<IActionResult> Create([FromBody] CreatePlayerDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto(400, "Failed.", "Redisplay form."));
            }

            var userId = this.GetCurrentUserId();
            var result = await _playerService.CreateAsync(dto, userId);

            return result.Succeeded
                ? Ok(new ResponseDto(200, "Completed.", "Player created."))
                : (IActionResult)BadRequest(new ResponseDto(400, "Failed.", result.Error));
        }

        /// <summary>
        /// Create new player
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>Response DTO</returns>
        [HttpPut]
        [Authorize(Policy = "RequireLoggedIn")]
        [ProducesResponseType(typeof(ResponseDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> Update([FromBody] UpdatePlayerDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto(400, "Failed.", "Redisplay form."));
            }

            var userId = this.GetCurrentUserId();
            var result = await _playerService.UpdateAsync(dto, userId);

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

        //Get List of all players by game with filers and sorting
        [HttpGet("players/pagination")]
        public async Task<PlayerIndexDto> PlayerIndex([FromQuery] PlayerControllerInputDto valuesFromUi)
        {
            PlayerInputValuesModelDto inputValues = new PlayerInputValuesModelDto()
            {
                GameId = valuesFromUi.IdGame,
                Page = valuesFromUi.Page,
                PageSize = valuesFromUi.PageSize,
                SortField = valuesFromUi.SortField,
                SortType = valuesFromUi.SortType,
                NameValue = valuesFromUi.NameValue,
                RankLeftValue = valuesFromUi.RankLeftValue,
                RankRightValue = valuesFromUi.RankRightValue,
                DecencyLeftValue = valuesFromUi.DecencyLeftValue,
                DecencyRightValue = valuesFromUi.DecencyRightValue
            };

            PlayerPaginationDto inputModel = await _playerService.GetFilteredPlayersByGameIdAsync(inputValues);

            List<PlayerListItemDto> players = new List<PlayerListItemDto>(inputModel.Players);
            if (players == null)
                return null;


            // representation model
            PlayerIndexDto viewModel = new PlayerIndexDto
            {
                PageViewModel = new PageDto(inputModel.PlayersQuantity, valuesFromUi.Page, valuesFromUi.PageSize),
                Players = players
            };

            return viewModel;
        }

        /// <summary>
        /// Get list of player's invitations
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        [ProducesResponseType(typeof(IList<InvitationListItemDto>), 200)]
        public async Task<IList<InvitationListItemDto>> InvitationPlayerListByUserId(int userId)
        {
            return await _playerService.GetAllPlayerInvitetionByUserId(userId);
        }
    }
}