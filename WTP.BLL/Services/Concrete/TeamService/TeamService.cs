using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WTP.BLL.DTOs.PlayerDTOs;
using WTP.BLL.DTOs.TeamDTOs;
using WTP.DAL.Entities;
using WTP.DAL.Entities.TeamEntities;
using WTP.DAL.UnitOfWork;

namespace WTP.BLL.Services.Concrete.TeamService
{
    public class TeamService : ITeamService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public TeamService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _uow = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TeamDto> GetTeamAsync(int teamId)
        {
            var team = await _uow.Teams.GetByIdAsync(teamId);
            var dto = _mapper.Map<TeamDto>(team);

            return dto;
        }

        public async Task<ServiceResult> CreateAsync(CreateOrUpdateTeamDto dto, int userId)
        {
            bool existedTeam = _uow.Teams.AsQueryable()
                                         .Any(t => t.Name == dto.Name &&
                                                   t.GameId == dto.GameId &&
                                                   t.CoachId != userId);

            if (existedTeam) return new ServiceResult("Team already existed.");

            var team = _mapper.Map<Team>(dto);
            team.CoachId = userId;

            await _uow.Teams.CreateOrUpdate(team);
            await _uow.CommitAsync();

            return new ServiceResult();
        }

        public async Task<ServiceResult> UpdateAsync(CreateOrUpdateTeamDto dto, int userId)
        {
            // check team
            var existedTeam = _uow.Teams.AsQueryable()
                                        .FirstOrDefault(t => t.CoachId == userId &&
                                                             t.GameId == dto.GameId);

            if (existedTeam == null) return new ServiceResult("Team not found.");

            existedTeam.GoalId = dto.GoalId;
            existedTeam.Name = dto.Name;
            existedTeam.ServerId = dto.ServerId;

            await _uow.Teams.CreateOrUpdate(existedTeam);
            await _uow.CommitAsync();

            return new ServiceResult();
        }

        public async Task<ServiceResult> DeleteAsync(int teamId, int userId)
        {
            var team = await _uow.Teams.AsQueryable()
                                       .FirstOrDefaultAsync(t => t.CoachId == userId &&
                                                                 t.Id == teamId);

            if (team == null) return new ServiceResult("Team not found.");

            await _uow.Teams.DeleteAsync(team.Id);
            await _uow.CommitAsync();

            return new ServiceResult();
        }

        public IList<PlayerListItemDto> GetPlayers(int teamId)
        {
            var players = _uow.Teams.AsQueryable()
                                    .FirstOrDefault(t => t.Id == teamId)
                                    .Players;

            return players == null
                ? null
                : _mapper.Map<IList<PlayerListItemDto>>(players);
        }

        public async Task<ServiceResult> InviteToPlayerAsync(TeamActionDto dto)
        {
            // check player
            var existedPlayer = await _uow.Players.AsQueryable()
                                                  .AnyAsync(p => p.Id == dto.PlayerId);

            if (!existedPlayer) return new ServiceResult("Player not found.");
            
            // check team
            var existedTeam = await _uow.Teams.AsQueryable()
                                              .AnyAsync(t => t.Id == dto.TeamId && 
                                                             t.CoachId == dto.UserId);

            if (!existedTeam) return new ServiceResult("Team not found.");

            // invite
            var invite = new Invitation
            {
                PlayerId = dto.PlayerId,
                TeamId = dto.TeamId,
                Author = Author.Coach
            };

            await _uow.Invitations.CreateOrUpdate(invite);
            await _uow.CommitAsync();

            return new ServiceResult();
        }

        public async Task<ServiceResult> InviteToTeamAsync(TeamActionDto dto)
        {
            // check player
            var existedPlayer = await _uow.Players.AsQueryable()
                                                  .AnyAsync(p => p.Id == dto.PlayerId && 
                                                                 p.AppUserId == dto.UserId);

            if (!existedPlayer) return new ServiceResult("Player not found.");

            // check team
            var existedTeam = await _uow.Teams.AsQueryable()
                                              .AnyAsync(t => t.Id == dto.TeamId);

            if (!existedTeam) return new ServiceResult("Team not found.");

            // invite
            var invite = new Invitation
            {
                PlayerId = dto.PlayerId,
                TeamId = dto.TeamId,
                Author = Author.Player
            };

            await _uow.Invitations.CreateOrUpdate(invite);
            await _uow.CommitAsync();

            return new ServiceResult();
        }

        public async Task<ServiceResult> AcceptInvitationAsync(InviteActionDto dto)
        {
            var invitation = await _uow.Invitations.GetByIdAsync(dto.InviteId);
            if (invitation == null) return new ServiceResult("Invitation not found.");

            //id
            int team = _uow.Teams.AsQueryable()
                                 .Select(t => t.Id)
                                 .FirstOrDefault(id => id == invitation.TeamId);

            if (team == 0) return new ServiceResult("Team not found.");

            if (!await CheckAuthor(invitation, dto.UserId)) return new ServiceResult("Error access.");

            return await AddToTeamAsync(invitation.PlayerId, invitation.TeamId);
        }

        public async Task<ServiceResult> DeclineInvitationAsync(InviteActionDto dto)
        {
            var invitation = await _uow.Invitations.GetByIdAsync(dto.InviteId);
            if (invitation == null) return new ServiceResult("Invitation not found.");

            if (!await CheckAuthor(invitation, dto.UserId)) return new ServiceResult("Error access.");

            await _uow.Invitations.DeleteAsync(invitation.Id);
            await _uow.CommitAsync();

            return new ServiceResult();
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

            team.Players.Add(player);

            await _uow.Teams.CreateOrUpdate(team);
            await _uow.CommitAsync();

            return new ServiceResult();
        }

        public async Task<ServiceResult> RemoveFromTeamAsync(TeamActionDto dto)
        {
            var team = await _uow.Teams.GetByIdAsync(dto.TeamId);
            if (team == null) return new ServiceResult("Team not found.");

            bool playerWithUserId(Player player) => player.Id == dto.UserId;
            if (team.CoachId != dto.UserId && !team.Players.Any(playerWithUserId))
                return new ServiceResult("You do not have access to perform this operation.");

            team.Players.Remove(team.Players.FirstOrDefault(playerWithUserId));

            await _uow.Teams.CreateOrUpdate(team);
            await _uow.CommitAsync();

            return new ServiceResult();
        }

        private async Task<bool> CheckAuthor(Invitation invitation, int authorId)
        {
            switch (invitation.Author)
            {
                case Author.Coach:
                    var existedPlayer = await _uow.Players.AsQueryable()
                                                          .AnyAsync(p => p.Id == invitation.PlayerId &&
                                                                         p.AppUserId == authorId);

                    if (existedPlayer) return true;
                    break;

                case Author.Player:
                    var existedTeam = await _uow.Teams.AsQueryable()
                                                      .AnyAsync(t => t.Id == invitation.TeamId &&
                                                                     t.CoachId == authorId);

                    if (existedTeam) return true;
                    break;
            }
            return false;
        }
    }
}
