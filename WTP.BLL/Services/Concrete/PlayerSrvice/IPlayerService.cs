using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WTP.BLL.DTOs.PlayerDTOs;
using WTP.BLL.DTOs.ServicesDTOs;

namespace WTP.BLL.Services.Concrete.PlayerSrvices
{
    public interface IPlayerService
    {
        Task CreateOrUpdateAsync(PlayerDto dto, int adminId=1);
        Task DeleteAsync(int playerId, int adminId = 1);
        Task<PlayerDto> FindAsync(int playerId);
        IQueryable<CommentDto> FindCommentsAsync(int playerId);
        IQueryable<MatchDto> FindMatchesAsync(int playerId);
        Task<IList<PlayerJoinedDto>> GetPlayersList();
    }
}
