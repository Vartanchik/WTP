using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WTP.BLL.DTOs.PlayerDTOs;

namespace WTP.BLL.Services.Concrete.PlayerSrvice
{
    public interface IPlayerService
    {
        Task CreateOrUpdateAsync(PlayerDto dto);
        Task DeleteAsync(int userId, int playerId);
        Task<PlayerDto> FindAsync(int playerId);
        IQueryable<CommentDto> FindCommentsAsync(int playerId);
        IQueryable<MatchDto> FindMatchesAsync(int playerId);
        Task<IList<PlayerListItemDto>> GetListByUserIdAsync(int userId);
        Task<IList<PlayerDto>> GetPlayersList();
    }
}
