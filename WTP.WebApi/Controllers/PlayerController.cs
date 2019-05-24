using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public PlayerController(IPlayerService playerService, IHistoryService historyService)
        {
            _playerService = playerService;
            _historyService = historyService;
        }

        ////Get List of all players
        [HttpGet]
        [Route("profile")]
        //[Authorize(Policy = "RequireAdministratorRole")]
        public async Task<PlayerDto[]> GetPlayersProfile()
        {
            var players = await _playerService.GetPlayersList();

            if (players == null)
                return null;

            return players.ToArray();
        }


        //Update players profile
        [HttpPut]
        //[Authorize(Policy = "RequireAdministratorRole")]
        [Route("player/{id}")]
        public async Task<IActionResult> UpdateUser([FromBody] PlayerDto formdata, [FromRoute]int id)
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

            List<string> errorList = new List<string>();


            var player = await _playerService.FindAsync(id);

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
            player.Server.Name = formdata.Server.Name;
            player.Name = formdata.Name;
            

            try
            {
                await _playerService.CreateOrUpdateAsync(player, adminId);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto
                {
                    StatusCode = 400,
                    Message = "Player's profile with id " + id + " wasnt updated.",
                    Info = ex.Message.ToString()
                });
            }

            return Ok(new ResponseDto
            {
                StatusCode = 202,
                Message = "Player's profile with id " + id + " was updated."
            });

        }

        //Delete players account by id
        [HttpDelete]
        //[Authorize(Policy = "RequireAdministratorRole")]
        [Route("player/{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute]int id)
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
                StatusCode = 202,
                Message = "Players profile with id " + id + " was deleted."
            });
        }
    }
}