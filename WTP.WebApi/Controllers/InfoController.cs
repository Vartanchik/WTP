using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WTP.BLL.DTOs.PlayerDTOs;
using WTP.BLL.DTOs.ServicesDTOs;
using WTP.BLL.Services.Concrete.GameService;

namespace WTP.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class InfoController : Controller
    {
        private readonly IGameService _gameService;

        public InfoController(IGameService gameService)
        {
            _gameService = gameService;
        }

        /// <summary>
        /// Get all games
        /// </summary>
        /// <returns>Enumeration of games</returns>
        /// <returns>Response DTO</returns>
        /// <response code="200">Returns list of games</response>
        /// <response code="400">Get all games failed</response>
        [HttpGet("[action]")]
        [ProducesResponseType(typeof(IEnumerable<GameDto>), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public IActionResult GetAllGames()
        {
            var listOfGames = _gameService.GetAllGames();

            return listOfGames == null
                ? BadRequest(new ResponseDto(400, "Get all games failed."))
                : (IActionResult)Ok(listOfGames);
        }
    }
}