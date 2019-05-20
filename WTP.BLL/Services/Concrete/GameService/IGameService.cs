using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WTP.BLL.DTOs.PlayerDTOs;

namespace WTP.BLL.Services.Concrete.GameService
{
    public interface IGameService
    {
        IEnumerable<GameDto> GetAllGames();
    }
}
