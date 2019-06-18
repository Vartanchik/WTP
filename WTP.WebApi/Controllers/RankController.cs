using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityFrameworkPaginateCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WTP.BLL.DTOs.PlayerDTOs;
using WTP.BLL.DTOs.ServicesDTOs;
using WTP.BLL.Services.Concrete.RankService;
using WTP.DAL.Entities;

namespace WTP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RankController : ControllerBase
    {
        private readonly IRankService _rankService;

        public RankController(IRankService rankService)
        {
            _rankService = rankService;
        }

        [HttpGet("list")]
        [Authorize(Policy = "RequireAdministratorRole")]
        public async Task<IList<RankDto>> GetAllRanks()
        {
            return await _rankService.GetRanksListAsync();
        }


        [HttpPost("item")]
        [Authorize(Policy = "RequireAdministratorRole")]
        public async Task<IActionResult> CreateRank([FromBody]RankDto rankDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto
                {
                    StatusCode = 400,
                    Message = "Model is not valid."
                });
            }

            await _rankService.CreateOrUpdateAsync(rankDto);

            return Ok(new ResponseDto
            {
                StatusCode = 201,
                Message = "Rank was created."
            });
        }

        [HttpPut("item/{rankId}")]
        [Authorize(Policy = "RequireAdministratorRole")]
        public async Task<IActionResult> EditRank([FromBody]RankDto rankDto, [FromRoute]int rankId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto
                {
                    StatusCode = 400,
                    Message = "Model is not valid."
                });
            }

            var rank = await _rankService.GetByIdAsync(rankId);
            if (rank == null)
                return NoContent();

            rank.Name = rankDto.Name;
            rank.Value = rankDto.Value;

            await _rankService.CreateOrUpdateAsync(rank);

            return Ok(new ResponseDto
            {
                StatusCode = 200,
                Message = "Rank with id " + rankId + " was updated."
            });
        }

        [HttpDelete("item/{rankId}")]
        [Authorize(Policy = "RequireAdministratorRole")]
        public async Task<IActionResult> DeleteRank([FromRoute]int rankId)
        {
            var rank = await _rankService.GetByIdAsync(rankId);

            if (rank == null)
                return NoContent();

            await _rankService.DeleteAsync(rankId);

            return Ok(new ResponseDto
            {
                StatusCode = 200,
                Message = "Rank with id " + rankId + " was deleted."
            });
        }

        [HttpGet]
        [Authorize(Policy = "RequireAdministratorRole")]
        [Route("ranks/paging")]
        public async Task<Page<Rank>> GetRecordsListOnPage(int pageSize, int currentPage, string sortBy,
                                        string name, int id, int value,
                                        bool sortOrder)
        {
            return await _rankService.GetFilteredSortedRanksOnPage(pageSize, currentPage, sortBy, name, id, value, sortOrder);
        }
    }
}