using EntityFrameworkPaginateCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.BLL.DTOs.PlayerDTOs;
using WTP.DAL.Entities;

namespace WTP.BLL.Services.Concrete.GameService
{
    public interface IGameService
    {
        Task<IList<GameDto>> GetAllGamesAsync();
        Task CreateOrUpdateAsync(GameDto dto, int? adminId = null);
        Task DeleteAsync(int gameId, int? adminId = null);
        Task<GameDto> FindAsync(int gameId);

        //For Admin
        Task<Page<Game>> GetFilteredSortedGamesOnPage(int pageSize, int currentPage, string sortBy
                                      , string name, int id, bool sortOrder);
    }
}
