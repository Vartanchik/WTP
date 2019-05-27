using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WTP.BLL.DTOs.PlayerDTOs;
using WTP.BLL.DTOs.ServicesDTOs;
using WTP.BLL.Services.Concrete.RankService;

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
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<RankDto[]> GetAllRanks()
        {
            var listOfranks = await _rankService.GetRanksListAsync();

            return listOfranks.ToArray();
        }


        [HttpPost("item")]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> CreateGame([FromBody]RankDto rankDto)
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
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> EditGame([FromBody]RankDto rankDto, [FromRoute]int rankId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto
                {
                    StatusCode = 400,
                    Message = "Model is not valid."
                });
            }

            var rank = await _rankService.FindAsync(rankId);
            if (rank == null)
                return NoContent();

            rank.Name = rankDto.Name;
            rank.Photo = rankDto.Photo;
            rank.GameId = rankDto.GameId;
            rank.Game = rankDto.Game;

            await _rankService.CreateOrUpdateAsync(rank);

            return Ok(new ResponseDto
            {
                StatusCode = 200,
                Message = "Rank with id " + rankId + " was updated."
            });
        }

        [HttpDelete("item/{rankId}")]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> DeleteGame([FromRoute]int rankId)
        {
            var rank = await _rankService.FindAsync(rankId);

            if (rank == null)
                return NoContent();

            await _rankService.DeleteAsync(rankId);

            return Ok(new ResponseDto
            {
                StatusCode = 200,
                Message = "Rank with id " + rankId + " was deleted."
            });
        }
    }
}