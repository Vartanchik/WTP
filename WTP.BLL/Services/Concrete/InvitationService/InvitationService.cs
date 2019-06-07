using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WTP.BLL.DTOs.TeamDTOs;
using WTP.DAL.Entities;
using WTP.DAL.Entities.TeamEntities;
using WTP.DAL.UnitOfWork;

namespace WTP.BLL.Services.Concrete.InvitationService
{
    public class InvitationService : IInvitationService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public InvitationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _uow = unitOfWork;
            _mapper = mapper;
        }

        public async Task<InvitationListItemDto> GetInvitationAsync(int invitationId)
        {
            var invitation = await _uow.Invitations.GetByIdAsync(invitationId);

            return invitation == null
                ? null
                : _mapper.Map<InvitationListItemDto>(invitation);
        }

        public async Task<List<InvitationListItemDto>> GetPlayerInvitationsAsync(int playerId)
        {
            var playerInvitations = await _uow.Invitations.AsQueryable()
                                                          .Where(i => i.PlayerId == playerId)
                                                          .ToListAsync();
            return playerInvitations == null
                ? null
                : _mapper.Map<List<InvitationListItemDto>>(playerInvitations);
        }

        public async Task<List<InvitationListItemDto>> GetTeamInvitationsAsync(int teamId)
        {
            var teamInvitations = await _uow.Invitations.AsQueryable()
                                                        .Where(i => i.TeamId == teamId)
                                                        .ToListAsync();

            return teamInvitations == null
                ? null
                : _mapper.Map<List<InvitationListItemDto>>(teamInvitations);
        }

        public async Task<ServiceResult> CreateInvitationAsync(TeamActionDto dto)
        {
            int playerUserId = await _uow.Players.AsQueryable()
                                                 .Where(p => p.Id == dto.PlayerId)
                                                 .Select(p => p.AppUserId)
                                                 .FirstOrDefaultAsync();

            if (playerUserId == 0) return new ServiceResult("Player not found.");

            var teamCoachId = await _uow.Teams.AsQueryable()
                                              .Where(t => t.Id == dto.TeamId)
                                              .Select(t => t.AppUserId)
                                              .FirstOrDefaultAsync();

            if (teamCoachId == 0) return new ServiceResult("Team not found.");

            Author author;

            if (playerUserId == dto.UserId) author = Author.Player;
            else if (teamCoachId == dto.UserId) author = Author.Coach;
            else return new ServiceResult("You do not have access to perform this operation.");

            var invite = new Invitation
            {
                PlayerId = dto.PlayerId,
                TeamId = dto.TeamId,
                Author = author
            };

            await _uow.Invitations.CreateOrUpdate(invite);
            await _uow.CommitAsync();

            return new ServiceResult();
        }

        public async Task<ServiceResult> DeclineInvitationAsync(InviteActionDto dto)
        {
            var invitation = await _uow.Invitations.GetByIdAsync(dto.InvitationId);

            if (invitation == null) return new ServiceResult("Invitation not found.");

            var userIsPlayer = await _uow.Players.AsQueryable()
                                                 .AnyAsync(p => p.Id == invitation.TeamId &&
                                                                p.AppUserId == dto.UserId);

            var userIsTeam = await _uow.Teams.AsQueryable()
                                             .AnyAsync(t => t.Id == invitation.TeamId &&
                                                            t.AppUserId == dto.UserId);

            if (!userIsPlayer && !userIsTeam) return new ServiceResult("You do not have access to perform this operation.");

            await _uow.Invitations.DeleteAsync(invitation.Id);
            await _uow.CommitAsync();

            return new ServiceResult();
        }

        public async Task<ServiceResult> AcceptInvitationAsync(InviteActionDto dto)
        {
            var invitation = await _uow.Invitations.GetByIdAsync(dto.InvitationId);

            if (invitation == null) return new ServiceResult("Invitation not found.");

            var playerUserId = await _uow.Players.AsQueryable()
                                                  .Where(p => p.AppUserId == invitation.PlayerId)
                                                  .Select(p => p.AppUserId)
                                                  .FirstOrDefaultAsync();

            if (playerUserId == 0) return new ServiceResult("Player not found.");

            var teamUserId = await _uow.Teams.AsQueryable()
                                              .Where(t => t.Id == invitation.TeamId)
                                              .Select(t => t.AppUserId)
                                              .FirstOrDefaultAsync();

            if (teamUserId == 0) return new ServiceResult("Team not found.");

            if (playerUserId == dto.UserId && invitation.Author == Author.Coach ||
                teamUserId == dto.UserId && invitation.Author == Author.Player)
            {
                return await AddToTeamAsync(invitation.PlayerId, invitation.TeamId);
            }
            else
            {
                return new ServiceResult("You do not have access to perform this operation.");
            }
        }

        private async Task<ServiceResult> AddToTeamAsync(int playerId, int teamId)
        {
            var team = await _uow.Teams.GetByIdAsync(teamId);

            if (team == null) return new ServiceResult("Team not found.");

            if (team.Players == null) team.Players = new List<Player>();
            else if (team.Players.Count >= 5) return new ServiceResult("Team is fuel.");

            var player = await _uow.Players.GetByIdAsync(playerId);

            if (player == null) return new ServiceResult("Player not found.");

            if (player.TeamId.GetValueOrDefault() != 0)
                return new ServiceResult("In order to state part of the team you must exit the current one.");

            player.Team = team;

            await _uow.Players.CreateOrUpdate(player);
            await _uow.CommitAsync();

            return new ServiceResult();
        }
    }
}
