using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WTP.BLL.DTOs.PlayerDTOs;
using WTP.BLL.DTOs.ServicesDTOs;
using WTP.BLL.Services.Concrete.PlayerSrvices;
using WTP.BLL.Shared;

namespace WTP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerManageController : ControllerBase
    {
        private readonly IPlayerService _playerService;
        private readonly IMapper _mapper;

        public PlayerManageController(IPlayerService playerService, IMapper mapper)
        {
            _playerService = playerService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Policy = "RequireAdministratorRole")]
        [Route("list")]
        public async Task<PlayerManageDto> GetPlayersList(string name, int page = 1, int pageSize = 3,
            PlayerSortState sortOrder = PlayerSortState.IdAsc)
        {
            return await _playerService.GetPageInfo(name,page,pageSize,sortOrder);//GetJoinedPlayersListAsync();
        }

        //ToDo
        [HttpPost]
        [Authorize(Policy = "RequireAdministratorRole")]
        [Route("item")]
        public async Task<IActionResult> CreatePlayer([FromBody] PlayerShortDto inputPlayer)
        {
            var result = await _playerService.CreateOrUpdateAsync(_mapper.Map<CreateUpdatePlayerDto>(inputPlayer),inputPlayer.AppUserId);
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
    }
}