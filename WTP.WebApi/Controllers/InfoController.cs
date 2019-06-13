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
        [HttpGet("[action]")]
        [ProducesResponseType(typeof(IList<GameDto>), 200)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> GetAllGames()
        {
            var listOfGames = await _gameService.GetAllGamesAsync();

            return listOfGames == null
                ? (IActionResult)NoContent()
                : Ok(listOfGames);
        }

        /// <summary>
        /// Get username and user photo
        /// </summary>
        /// <param name="userId"></param>
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