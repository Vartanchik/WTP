using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WTP.BLL.DTOs.PlayerDTOs;
using WTP.BLL.DTOs.ServicesDTOs;

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
        Task<IQueryable<PlayerJoinedDto>> GetJoinedPlayersList();
        Task<IList<PlayerDto>> GetPlayers();
        Task<IList<PlayerJoinedDto>> GetAllPlayersList();
        Task<PlayerJoinedDto> GetPlayerInfo(int playerId);
    }
}
