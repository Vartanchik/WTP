﻿using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.BLL.DTOs.PlayerDTOs;
using WTP.BLL.DTOs.TeamDTOs;

namespace WTP.BLL.Services.Concrete.TeamService
{
    public interface ITeamService
    {
        Task<TeamDto> GetTeamAsync(int teamId);
        Task<ServiceResult> CreateAsync(CreateOrUpdateTeamDto dto, int userId);
        Task<ServiceResult> UpdateAsync(CreateOrUpdateTeamDto dto, int userId);
        Task<ServiceResult> DeleteAsync(int teamId, int userId);
        IList<PlayerListItemDto> GetPlayers(int teamId);
        Task<ServiceResult> InviteToPlayerAsync(TeamActionDto dto);
        Task<ServiceResult> InviteToTeamAsync(TeamActionDto dto);
        Task<ServiceResult> AcceptInvitation(InviteActionDto dto);
        Task<ServiceResult> DeclineInvitation(InviteActionDto dto);
        Task<ServiceResult> AddToTeam(int playerId, int teamId);
        Task<ServiceResult> RemoveFromTeam(TeamActionDto dto);
    }
}
