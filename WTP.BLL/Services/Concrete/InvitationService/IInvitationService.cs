using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.BLL.DTOs.TeamDTOs;

namespace WTP.BLL.Services.Concrete.InvitationService
{
    public interface IInvitationService
    {
        Task<InvitationListItemDto> GetInvitationAsync(int invitationId);
        Task<List<InvitationListItemDto>> GetPlayerInvitationsAsync(int playerId);
        Task<List<InvitationListItemDto>> GetTeamInvitationsAsync(int teamId);
        Task<ServiceResult> CreateInvitationAsync(TeamActionDto dto);
        Task<ServiceResult> DeclineInvitationAsync(InviteActionDto dto);
        Task<ServiceResult> AcceptInvitationAsync(InviteActionDto dto);
    }
}
