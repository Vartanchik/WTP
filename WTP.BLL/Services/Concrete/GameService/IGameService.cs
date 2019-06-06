using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTP.BLL.DTOs.PlayerDTOs;

namespace WTP.BLL.Services.Concrete.GameService
{
    public interface IGameService
    {
        IEnumerable<GameDto> GetAllGames();
        Task<IList<GameDto>> GetGamesListAsync();
        Task CreateOrUpdateAsync(GameDto dto, int? adminId = null);
        Task DeleteAsync(int gameId, int? adminId = null);
        Task<GameDto> FindAsync(int gameId);
        Task<GameDto> GetByIdAsync(int gameId);
    }
}
