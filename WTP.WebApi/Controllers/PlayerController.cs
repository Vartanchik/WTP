using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WTP.BLL.DTOs.AppUserDTOs;
using WTP.BLL.DTOs.PlayerDTOs;
using WTP.BLL.DTOs.ServicesDTOs;
using WTP.BLL.Services.Concrete.PlayerSrvices;
using WTP.BLL.Services.HistoryService;

namespace WTP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly IHistoryService _historyService;
        private readonly IPlayerService _playerService;
        private readonly IMapper _mapper;

        public PlayerController(IPlayerService playerService, IHistoryService historyService, IMapper mapper)
        {
            _playerService = playerService;
            _historyService = historyService;
            _mapper = mapper;
        }

        ////Get List of all players
        [HttpGet]
        [Route("list")]
        //[Authorize(Policy = "RequireAdministratorRole")]
        public async Task<PlayerJoinedDto[]> GetPlayersProfile()
        {
            var players = await _playerService.GetAllPlayersList();

            if (players == null)
                return null;

            return players.ToArray();
        }


        //Create players profile
        [HttpPost]
        //[Authorize(Policy = "RequireAdministratorRole")]
        [Route("item")]
        public async Task<IActionResult> CreateProfile([FromBody] PlayerShortDto formdata)
        {
            //int userId = Convert.ToInt32(User.Claims.First(c => c.Type == "UserID").Value);
            int adminId = 1;

            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto
                {
                    StatusCode = 400,
                    Message = "Model is not valid."
                });
            }

            formdata.ServerId = formdata.Server.Id;
            formdata.AppUserId = formdata.AppUser.Id;
            formdata.RankId= formdata.Rank.Id;
            formdata.GoalId = formdata.Goal.Id;
            formdata.GameId = formdata.Game.Id;
            formdata.TeamId = formdata.Team.Id;

            var newplayer = _mapper.Map<PlayerDto>(formdata);
            await _playerService.CreateOrUpdateAsync(newplayer, adminId);

            return Ok(new ResponseDto
            {
                StatusCode = 201,
                Message = "Player's profile was created."
            });

        }

        //Update players profile
        [HttpPut]
        //[Authorize(Policy = "RequireAdministratorRole")]
        [Route("item/{id}")]
        public async Task<IActionResult> UpdateProfile([FromBody] PlayerJoinedDto formdata, [FromRoute]int id)
        {
            //int userId = Convert.ToInt32(User.Claims.First(c => c.Type == "UserID").Value);
            int adminId = 1;

            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto
                {
                    StatusCode = 400,
                    Message = "Model is not valid."
                });
            }

            var player = await _playerService.GetPlayerInfo(id);

            if (player == null)
            {
                return NotFound(new ResponseDto
                {
                    StatusCode = 404,
                    Message = "Player with id:" + id + " wasnt found.",
                    Info = "Null"
                });
            }

            player.About = formdata.About;
            player.Decency = formdata.Decency;
            player.Server = formdata.Server;
            player.Name = formdata.Name;
            player.AppUser.Email = formdata.AppUser.Email;
            player.AppUser.Id = formdata.AppUser.Id;
            player.AppUser.UserName = formdata.AppUser.UserName;
            player.Game = formdata.Game;
            player.Goal = formdata.Goal;
            player.Rank = formdata.Rank;
            player.Server = formdata.Server;
            player.Team = formdata.Team;

            var newplayer = _mapper.Map<PlayerDto>(player);
            await _playerService.CreateOrUpdateAsync(newplayer, adminId);

            return Ok(new ResponseDto
            {
                StatusCode = 200,
                Message = "Player's profile with id " + id + " was updated."
            });

        }

        //Delete players account by id
        [HttpDelete]
        //[Authorize(Policy = "RequireAdministratorRole")]
        [Route("item/{id}")]
        public async Task<IActionResult> DeleteProfile([FromRoute]int id)
        {
            //int userId = Convert.ToInt32(User.Claims.First(c => c.Type == "UserID").Value);
            int adminId = 1;
            try
            {
                await _playerService.DeleteAsync(id, adminId);
            }
            catch (Exception ex)
            {
                return NotFound(new ResponseDto
                {
                    StatusCode = 404,
                    Message = "User's profile with id " + id + " wasn't found.",
                    Info = ex.Message.ToString()
                });
            }

            return Ok(new ResponseDto
            {
                StatusCode = 200,
                Message = "Players profile with id " + id + " was deleted."
            });
        }
    }
}