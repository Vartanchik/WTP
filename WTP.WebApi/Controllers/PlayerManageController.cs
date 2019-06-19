using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EntityFrameworkPaginateCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WTP.BLL.DTOs.PlayerDTOs;
using WTP.BLL.DTOs.ServicesDTOs;
using WTP.BLL.Services.Concrete.AdminPlayerService;
using WTP.BLL.Services.Concrete.PlayerSrvice;
using WTP.BLL.Shared;
using WTP.DAL.Entities;

namespace WTP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerManageController : ControllerBase
    {
        private readonly IAdminPlayerService _adminPlayerService;
        private readonly IPlayerService _playerService;
        private readonly IMapper _mapper;

        public PlayerManageController(IPlayerService playerService, IMapper mapper, IAdminPlayerService adminPlayerService)
        {
            _playerService = playerService;
            _adminPlayerService = adminPlayerService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Policy = "RequireAdministratorRole")]
        [Route("list")]
        public async Task<PlayerManageDto> GetPlayersList(string name, int page = 1, int pageSize = 3,
            PlayerSortState sortOrder = PlayerSortState.IdAsc)
        {
            return await _adminPlayerService.GetPageInfo(name, page, pageSize, sortOrder);
        }

        [HttpGet]
        [Authorize(Policy = "RequireAdministratorRole")]
        [Route("paging")]
        public async Task<Page<Player>> GetPlayersListOnPage(int pageSize, int currentPage, string sortBy
                                       , string playerName, string userName, string email,
                                        string gameName, string teamName, string rankName, string goalName, bool sortOrder)
        {
            return await _adminPlayerService.GetFilteredSortedPlayersOnPage(pageSize,currentPage,sortBy,playerName,userName,
                email,gameName,teamName,rankName,goalName,sortOrder);
        }

        [HttpPost]
        [Authorize(Policy = "RequireAdministratorRole")]
        [Route("item")]
        public async Task<IActionResult> CreatePlayer([FromBody] PlayerShortDto inputPlayer)
        {
            var result = await _playerService.CreateAsync(_mapper.Map<CreatePlayerDto>(inputPlayer), inputPlayer.AppUserId);
            if (result.Succeeded)
                return Ok(new ResponseDto
                {
                    StatusCode = 201,
                    Message = "Player with id " + inputPlayer.Id + " was created.",
                    Info = "Success."
                });

            return NotFound(new ResponseDto
            {
                StatusCode = 404,
                Message = "Player with id " + inputPlayer.Id + " wasn't found."
            });
        }


        //GetJoinedPlayersListAsync
        [HttpGet]
        [Authorize(Policy = "RequireAdministratorRole")]
        [Route("list/info")]
        public async Task<IList<PlayerShortDto>> GetJoinedPlayersList()
        {
            return await _adminPlayerService.GetJoinedPlayersListAsync();
        }

        [HttpPut]
        [Authorize(Policy = "RequireAdministratorRole")]
        [Route("item")]
        public async Task<IActionResult> EditPlayer([FromBody] PlayerShortDto inputPlayer)
        {
            var result = await _playerService.UpdateAsync(_mapper.Map<UpdatePlayerDto>(inputPlayer), inputPlayer.AppUserId);
            if (result.Succeeded)
                return Ok(new ResponseDto
                {
                    StatusCode = 201,
                    Message = "Player with id " + inputPlayer.Id + " was updated.",
                    Info = "Success."
                });

            return NotFound(new ResponseDto
            {
                StatusCode = 404,
                Message = "Player with id " + inputPlayer.Id + " wasn't found."
            });
        }

        [HttpDelete]
        [Authorize(Policy = "RequireAdministratorRole")]
        [Route("item")]
        public async Task<IActionResult> DeletePlayer([FromBody] PlayerShortDto inputPlayer)
        {
            if (ModelState.IsValid)
            {
                var result = await _playerService.DeleteAsync(inputPlayer.AppUserId, inputPlayer.GameId);

                if (result.Succeeded)
                    return Ok(new ResponseDto
                    {
                        StatusCode = 201,
                        Message = "Player with id " + inputPlayer.Id + " was deleted.",
                        Info = "Success."
                    });

                return NotFound(new ResponseDto
                {
                    StatusCode = 404,
                    Message = "Player with id " + inputPlayer.Id + " wasn't found."
                });
            }
            else
            {
                return BadRequest(new ResponseDto
                {
                    StatusCode = 400,
                    Message = "Model is not valid.",
                    Info = "BadRequest."
                });
            }
        }
    }
}