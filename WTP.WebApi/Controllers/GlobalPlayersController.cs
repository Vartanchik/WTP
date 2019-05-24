using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WTP.BLL.DTOs.PlayerDTOs;
using WTP.BLL.Shared;

namespace WTP.WebAPI.Controllers
{
    public class GlobalPlayersController : ControllerBase
    {
        [HttpGet]
        [Route("users/pagination")]
        public async Task<IActionResult> UserIndex(string name, int page = 1,
            SortState sortOrder = SortState.NameAsc, bool enableDeleted = true, bool enableLocked = true)
        {
            int pageSize = 3;

            List<PlayerListItemDto> users = new List<PlayerListItemDto>(await _appUserService.GetAllUsersAsync());
            if (users == null)
                return NoContent();

            //Filtration
            users = _appUserService.Filter(users, name);

            // Sorting
            users = _appUserService.Sort(users, sortOrder, enableDeleted, enableLocked);

            // Pagination
            var count = users.Count();
            var items = users.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // representation model
            UserIndexDto viewModel = new UserIndexDto
            {
                PageViewModel = new PageDto(count, page, pageSize),
                SortViewModel = new UserSortDto(sortOrder),
                //FilterViewModel = new UserFilterViewModel(users/*(List<AppUserDto>)await _appUserService.GetAllUsersAsync()*/, name),
                Users = items
            };
            return Ok(viewModel);
        }

    }
}