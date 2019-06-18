using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WTP.BLL.DTOs.PlayerDTOs;
using WTP.BLL.DTOs.ServicesDTOs;
using WTP.BLL.DTOs.TeamDTOs;
using WTP.BLL.Shared;

namespace WTP.BLL.Services.Concrete.PlayerSrvice
{
    public interface IPlayerService
    {
        Task<PlayerDto> GetPlayerAsync(int playerId);
        Task<ServiceResult> CreateAsync(CreatePlayerDto dto, int userId);
        Task<ServiceResult> UpdateAsync(UpdatePlayerDto dto, int userId);
        Task<ServiceResult> DeleteAsync(int userId, int playerGameId);
        Task<PlayerDto> FindAsync(int playerId);
        IQueryable<CommentDto> FindCommentsAsync(int playerId);
        IQueryable<MatchDto> FindMatchesAsync(int playerId);
        Task<IList<PlayerListItemDto>> GetListByUserIdAsync(int userId);
        Task<IList<PlayerListItemDto>> GetPlayersList();
        Task<IList<PlayerListItemDto>> GetListByTeamIdAsync(int teamId);
        Task<PlayerPaginationDto> GetFilteredPlayersByGameIdAsync(PlayerInputValuesModelDto inputValues);
        Task<IList<InvitationListItemDto>> GetAllPlayerInvitetionByUserId(int userId);
    }
}
