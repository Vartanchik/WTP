using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.BLL.DTOs.PlayerDTOs;
using WTP.BLL.DTOs.TeamDTOs;

namespace WTP.BLL.Services.Concrete.TeamService
{
    public interface ITeamService
    {
        Task<TeamDto> GetTeamAsync(int teamId);
        Task<ServiceResult> CreateAsync(CreateTeamDto dto, int userId);
        Task<ServiceResult> UpdateAsync(UpdateTeamDto dto, int userId);
        Task<ServiceResult> DeleteAsync(int teamId, int userId);
        Task<IList<PlayerListItemDto>> GetTeamPlayers(int teamId);
        Task<ServiceResult> CreateInvitationAsync(TeamActionDto dto);
        Task<ServiceResult> DeclineInvitationAsync(InviteActionDto dto);
        Task<ServiceResult> AcceptInvitationAsync(InviteActionDto dto);
        Task<ServiceResult> RemoveFromTeamAsync(TeamActionDto dto);
        Task<IList<TeamListItemDto>> GetListByUserIdAsync(int userId);
        Task<ServiceResult> UpdateLogoAsync(int userId, int teamId, string logo);
        Task<IList<InvitationListItemDto>> GetAllTeamInvitetionByUserId(int userId);
    }
}
