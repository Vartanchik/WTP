using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.BLL.DTOs.PlayerDTOs;

namespace WTP.BLL.Services.Concrete.GameService
{
    public interface IGameService
    {
        Task<IList<GameDto>> GetAllGamesAsync();
        Task CreateOrUpdateAsync(GameDto dto, int? adminId = null);
        Task DeleteAsync(int gameId, int? adminId = null);
        Task<GameDto> FindAsync(int gameId);
    }
}
