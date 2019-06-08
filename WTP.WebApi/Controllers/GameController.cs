﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WTP.BLL.DTOs.PlayerDTOs;
using WTP.BLL.DTOs.ServicesDTOs;
using WTP.BLL.Services.Concrete.GameService;

namespace WTP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet("list")]
        [Authorize(Policy = "RequireAdministratorRole")]
        public async Task<GameDto[]> GetAllGames()
        {
            var listOfGames = await _gameService.GetAllGamesAsync();

            return listOfGames.ToArray();
        }

        [HttpPost("item")]
        [Authorize(Policy = "RequireAdministratorRole")]
        public async Task<IActionResult> CreateGame([FromBody]GameDto gameDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto
                {
                    StatusCode = 400,
                    Message = "Model is not valid."
                });
            }

            await _gameService.CreateOrUpdateAsync(gameDto);

            return Ok(new ResponseDto
            {
                StatusCode = 201,
                Message = "Game was created."
            });
        }

        [HttpPut("item/{gameId}")]
        [Authorize(Policy = "RequireAdministratorRole")]
        public async Task<IActionResult> EditGame([FromBody]GameDto gameDto, [FromRoute]int gameId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto
                {
                    StatusCode = 400,
                    Message = "Model is not valid."
                });
            }

            var game = await _gameService.FindAsync(gameId);
            if (game == null)
                return NoContent();

            game.Name = gameDto.Name;
            await _gameService.CreateOrUpdateAsync(game);

            return Ok(new ResponseDto
            {
                StatusCode = 200,
                Message = "Game with id " + gameId + " was updated."
            });
        }

        [HttpDelete("item/{gameId}")]
        [Authorize(Policy = "RequireAdministratorRole")]
        public async Task<IActionResult> DeleteGame([FromRoute]int gameId)
        {
            var game = await _gameService.FindAsync(gameId);

            if (game == null)
                return NoContent();

            await _gameService.DeleteAsync(gameId);

            return Ok(new ResponseDto
            {
                StatusCode = 200,
                Message = "Game with id " + gameId + " was deleted."
            });
        }
    }
}