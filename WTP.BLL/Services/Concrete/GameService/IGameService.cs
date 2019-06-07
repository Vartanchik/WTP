using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.BLL.DTOs.PlayerDTOs;

namespace WTP.BLL.Services.Concrete.GameService
{
    public interface IGameService
    {
        Task<IList<GameDto>> GetAllGamesAsync();
    }
}
