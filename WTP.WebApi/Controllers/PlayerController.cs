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
        /// Get player by id
        /// </summary>
        /// <param name="playerId"></param>
        [HttpGet("{playerId}")]
        [ProducesResponseType(typeof(PlayerDto), 200)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Get([FromRoute] int playerId)
        {
            var player = await _playerService.GetPlayerAsync(playerId);

            return player == null
                ? (IActionResult)NoContent()
                : Ok(player);
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
        [HttpDelete("{gameId}")]
        [Authorize(Policy = "RequireLoggedIn")]
        [ProducesResponseType(typeof(ResponseDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> Delete([FromRoute] int gameId)
        {
            var userId = this.GetCurrentUserId();

            var result = await _playerService.DeleteAsync(userId, gameId);

            return result.Succeeded
                ? Ok(new ResponseDto(200, "Completed.", "Player deleted."))
                : (IActionResult)BadRequest(new ResponseDto(400, "Failed.", result.Error));
        }

        /// <summary>
        /// Get all user's players 
        /// </summary>
        /// <returns>List of players</returns>
        /// <returns>List<PlayerListItemDto></returns>
        [HttpGet("[action]/{userId}")]
        [ProducesResponseType(typeof(IList<PlayerListItemDto>), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> UserPlayers([FromRoute] int userId)
        {
            var players = await _playerService.GetListByUserIdAsync(userId);

            return players == null
                ? (IActionResult)NoContent()
                : Ok(players);
        }


        /// <summary>
        /// Get list players by team
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns>List<PlayerListItemDto></returns>
        [HttpGet("[action]/{teamId}")]
        [ProducesResponseType(typeof(IList<PlayerListItemDto>), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> TeamPlayers([FromRoute]int teamId)
        {
            var teams = await _playerService.GetListByTeamIdAsync(teamId);

            return teams == null
                ? (IActionResult)NoContent()
                : Ok(teams);
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
    }
}