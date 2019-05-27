using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WTP.BLL.DTOs.ServicesDTOs;
using WTP.BLL.DTOs.TeamDTOs;
using WTP.BLL.Services.Concrete.TeamService;

namespace WTP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _teamService;

        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpGet("list")]
        public async Task<TeamDto[]> GetAllTeams()
        {
            var listOfTeams = await _teamService.GetTeamsListAsync();

            return listOfTeams.ToArray();
        }


        [HttpPost("item")]
        public async Task<IActionResult> CreateTeam([FromBody]TeamDto teamDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto
                {
                    StatusCode = 400,
                    Message = "Model is not valid."
                });
            }

            await _teamService.CreateOrUpdateAsync(teamDto);

            return Ok(new ResponseDto
            {
                StatusCode = 201,
                Message = "Team was created."
            });
        }

        [HttpPut("item/{teamId}")]
        public async Task<IActionResult> EditTeam([FromBody]TeamDto teamDto, [FromRoute]int teamId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto
                {
                    StatusCode = 400,
                    Message = "Model is not valid."
                });
            }

            var team = await _teamService.FindAsync(teamId);

            if (team == null)
                return NoContent();

            team.Name = teamDto.Name;

            await _teamService.CreateOrUpdateAsync(team);

            return Ok(new ResponseDto
            {
                StatusCode = 200,
                Message = "Team with id " + teamId + " was updated."
            });
        }

        [HttpDelete("item/{teamId}")]
        public async Task<IActionResult> DeleteGame([FromRoute]int teamId)
        {
            var rank = await _teamService.FindAsync(teamId);

            if (rank == null)
                return NoContent();

            await _teamService.DeleteAsync(teamId);

            return Ok(new ResponseDto
            {
                StatusCode = 200,
                Message = "Team with id " + teamId + " was deleted."
            });
        }
    }
}