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
            bool existed = _uow.Teams.AsQueryable()
                                     .Any(t => t.Name == dto.Name &&
                                               t.GameId == dto.GameId &&
                                               t.CoachId != userId);

            if (existed) return new ServiceResult("Team already existed.");

            var team = _mapper.Map<Team>(dto);
            team.CoachId = userId;

            await _uow.Teams.CreateOrUpdate(team);
            await _uow.CommitAsync();

            return new ServiceResult();
        }

        public async Task<ServiceResult> UpdateAsync(CreateOrUpdateTeamDto dto, int userId)
        {
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

        public async Task<ServiceResult> DeleteAsync(int userId, int teamId)
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
            var players = _uow.Teams.AsQueryable().FirstOrDefault(t => t.Id == teamId).Players;

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

            // create invitation
            var invite = new Invitations {
                PlayerId = dto.PlayerId,
                TeamId = dto.TeamId,
                Author = Invitation.Coach
            };

            await _uow.Invitations.CreateOrUpdate(invite);
            await _uow.CommitAsync();

            return new ServiceResult();
        }

        public async Task<ServiceResult> InviteToTeamAsync(TeamActionDto dto)
        {
            //
            var existedPlayer = await _uow.Players.AsQueryable()
                                                  .AnyAsync(p => p.Id == dto.PlayerId && 
                                                                 p.AppUserId == dto.UserId);

            if (!existedPlayer) return new ServiceResult("Player not found.");

            //
            var existedTeam = await _uow.Teams.AsQueryable()
                                              .AnyAsync(t => t.Id == dto.TeamId);

            if (!existedTeam) return new ServiceResult("Team not found.");

            // create invitation
            var invite = new Invitations
            {
                PlayerId = dto.PlayerId,
                TeamId = dto.TeamId,
                Author = Invitation.Player
            };

            await _uow.Invitations.CreateOrUpdate(invite);
            await _uow.CommitAsync();

            return new ServiceResult();
        }

        public Task<ServiceResult> AcceptInvitation(InviteActionDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> DeclineInvitation(InviteActionDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> AddToTeam(TeamActionDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> RemoveFromTeam(TeamActionDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
