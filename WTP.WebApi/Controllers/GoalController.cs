using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WTP.BLL.DTOs.PlayerDTOs;
using WTP.BLL.DTOs.ServicesDTOs;
using WTP.BLL.Services.Concrete.GoalService;

namespace WTP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoalController : ControllerBase
    {
        private readonly IGoalService _goalService;

        public GoalController(IGoalService goalService)
        {
            _goalService = goalService;
        }

        [HttpGet("list")]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<GoalDto[]> GetAllGoals()
        {
            var listOfGoals = await _goalService.GetGoalsListAsync();

            return listOfGoals.ToArray();
        }


        [HttpPost("item")]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> CreateTeam([FromBody]GoalDto goalDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto
                {
                    StatusCode = 400,
                    Message = "Model is not valid."
                });
            }

            await _goalService.CreateOrUpdateAsync(goalDto);

            return Ok(new ResponseDto
            {
                StatusCode = 201,
                Message = "Goal was created."
            });
        }

        [HttpPut("item/{goalId}")]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> EditTeam([FromBody]GoalDto goalDto, [FromRoute]int goalId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto
                {
                    StatusCode = 400,
                    Message = "Model is not valid."
                });
            }

            var goal = await _goalService.FindAsync(goalId);

            if (goal == null)
                return NoContent();

            goal.Name = goalDto.Name;

            await _goalService.CreateOrUpdateAsync(goal);

            return Ok(new ResponseDto
            {
                StatusCode = 200,
                Message = "Goal with id " + goalId + " was updated."
            });
        }

        [HttpDelete("item/{goalId}")]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> DeleteGame([FromRoute]int goalId)
        {
            var goal = await _goalService.FindAsync(goalId);

            if (goal == null)
                return NoContent();

            await _goalService.DeleteAsync(goalId);

            return Ok(new ResponseDto
            {
                StatusCode = 200,
                Message = "Goal with id " + goalId + " was deleted."
            });
        }
    }
}