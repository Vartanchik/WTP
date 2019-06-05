using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WTP.BLL.DTOs.PlayerDTOs;
using WTP.BLL.DTOs.ServicesDTOs;
using WTP.BLL.Services.Concrete.AppUserService;
using WTP.BLL.Services.Concrete.GameService;

namespace WTP.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class InfoController : Controller
    {
        private readonly IGameService _gameService;
        private readonly IAppUserService _appUserService;
        private readonly IConfiguration _configuration;

        public InfoController(IGameService gameService, IAppUserService appUserService, IConfiguration configuration)
        {
            _gameService = gameService;
            _appUserService = appUserService;
            _configuration = configuration;
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

        [HttpGet("[action]/{userId}")]
        [ProducesResponseType(typeof(UserIconDto), 200)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UserIcon([FromRoute] int userId)
        {
            var icon = await _appUserService.GetUserIconAsync(userId);

            switch (icon)
            {
                case null:
                    return NoContent();
                default:
                    if (icon.Photo == null) icon.Photo = _configuration["Photo:DefaultPhoto"];
                    return Ok(icon);
            }
        }
    }
}