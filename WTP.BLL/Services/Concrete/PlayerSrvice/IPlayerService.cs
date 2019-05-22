using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WTP.BLL.DTOs.PlayerDTOs;

namespace WTP.BLL.Services.Concrete.PlayerSrvice
{
    public interface IPlayerService
    {
        Task CreateOrUpdateAsync(PlayerDto dto);
        Task DeleteAsync(int playerId);
        Task<PlayerDto> FindAsync(int playerId);
        IQueryable<CommentDto> FindCommentsAsync(int playerId);
        IQueryable<MatchDto> FindMatchesAsync(int playerId);
        IList<PlayerDto> GetPlayersByUserId(int userId);
    }
}
