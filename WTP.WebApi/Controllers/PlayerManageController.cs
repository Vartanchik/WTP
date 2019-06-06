using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WTP.BLL.DTOs.PlayerDTOs;
using WTP.BLL.Services.Concrete.PlayerSrvices;
using WTP.BLL.Shared;

namespace WTP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerManageController : ControllerBase
    {
        private readonly IPlayerService _playerService;

        public PlayerManageController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpGet]
        //[Authorize(Policy = "RequireAdministratorRole")]
        [Route("players/list")]
        public async Task<PlayerManageDto> GetPlayersList(string name, int page = 1, int pageSize = 3,
            PlayerSortState sortOrder = PlayerSortState.IdAsc)
        {
            return await _playerService.GetPageInfo(name,page,pageSize,sortOrder);//GetJoinedPlayersListAsync();
        }
    }
}