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
                                                          .Select(i => new InvitationListItemDto
                                                          {
                                                              Id = i.Id,
                                                              PlayerName = i.Player.Name,
                                                              TeamName = i.Team.Name,
                                                              Author = i.Author.ToString()
                                                          })
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
            var playerChecker = await _uow.Players.AsQueryable()
                                                 .Where(p => p.Id == dto.PlayerId)
                                                 .Select(p => new { p.AppUserId, p.GameId })
                                                 .FirstOrDefaultAsync();

            if (playerChecker == null) return new ServiceResult("Player not found.");

            var teamChecker = await _uow.Teams.AsQueryable()
                                              .Where(t => t.Id == dto.TeamId)
                                              .Select(t => new { t.AppUserId, t.GameId, t.Players.Count })
                                              .FirstOrDefaultAsync();

            if (teamChecker == null) return new ServiceResult("Team not found.");

            if (playerChecker.GameId != teamChecker.GameId) return new ServiceResult("Player and team must be from same game.");

            var exist = await _uow.Invitations.AsQueryable()
                                              .AnyAsync(i => i.PlayerId == dto.PlayerId &&
                                                             i.TeamId == dto.TeamId);

            if (exist) return new ServiceResult("Invitation has already been sent.");

            Author author;

            if (playerChecker.AppUserId == dto.UserId) author = Author.Player;
            else if (teamChecker.AppUserId == dto.UserId)
            {
                if (teamChecker.Count >= 5) return new ServiceResult("Team is full.");
                author = Author.Coach;
            }
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
