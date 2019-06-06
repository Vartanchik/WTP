using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WTP.BLL.DTOs.PlayerDTOs;
using WTP.BLL.DTOs.ServicesDTOs;
using WTP.BLL.Shared;
using WTP.DAL.Entities;

namespace WTP.BLL.Services.Concrete.PlayerSrvices
{
    public interface IPlayerService
    {
        Task<ServiceResult> CreateOrUpdateAsync(CreateUpdatePlayerDto dto, int userId);
        Task<ServiceResult> DeleteAsync(int userId, int playerGameId);
        Task<PlayerDto> FindAsync(int playerId);
        IQueryable<CommentDto> FindCommentsAsync(int playerId);
        IQueryable<MatchDto> FindMatchesAsync(int playerId);
        Task<IList<PlayerListItemDto>> GetListByUserIdAsync(int userId);
        Task<IList<PlayerListItemDto>> GetPlayersList();
        Task<IList<PlayerListItemDto>> GetListByGameIdAsync(int gameId);

        Task<IList<PlayerShortDto>> GetJoinedPlayersListAsync();

        IQueryable<Player> GetItemsOnPage(int page, int pageSize, IQueryable<Player> baseQuery);
        Task<int> GetCountOfPlayers();
        IQueryable<Player> FilterByName(string name, IQueryable<Player> baseQuery);
        IQueryable<Player> SortByParam(PlayerSortState sortOrder, IQueryable<Player> baseQuery);
        Task<PlayerManageDto> GetPageInfo(string name, int page, int pageSize,
            PlayerSortState sortOrder);
    }
}
