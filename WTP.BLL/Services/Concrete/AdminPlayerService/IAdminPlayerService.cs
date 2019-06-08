using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTP.BLL.DTOs.PlayerDTOs;
using WTP.BLL.Shared;
using WTP.DAL.Entities;

namespace WTP.BLL.Services.Concrete.AdminPlayerService
{
    public interface IAdminPlayerService
    {
        Task<IList<PlayerShortDto>> GetJoinedPlayersListAsync();
        IQueryable<Player> GetItemsOnPage(int page, int pageSize, IQueryable<Player> baseQuery);
        Task<int> GetCountOfPlayers();
        IQueryable<Player> FilterByName(string name, IQueryable<Player> baseQuery);
        IQueryable<Player> SortByParam(PlayerSortState sortOrder, IQueryable<Player> baseQuery);
        Task<PlayerManageDto> GetPageInfo(string name, int page, int pageSize,
            PlayerSortState sortOrder);
    }
}
