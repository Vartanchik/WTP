﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WTP.BLL.DTOs.PlayerDTOs;
using WTP.BLL.DTOs.TeamDTOs;
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
            var team = await _uow.Teams.AsQueryable()
                                       .Include(t => t.Game)
                                       .Include(t => t.Server)
                                       .Include(t => t.Goal)
                                       .Include(t => t.Players)
                                         .ThenInclude(p => p.Rank)
                                       .Include(t => t.Players)
                                         .ThenInclude(p => p.AppUser)
                                       .Where(t => t.Id == teamId)
                                       .FirstOrDefaultAsync();

            var dto = _mapper.Map<TeamDto>(team);

            return dto;
        }

        public async Task<ServiceResult> CreateAsync(CreateTeamDto dto, int userId)
        {
            bool existedTeam = await _uow.Teams.AsQueryable()
                                               .AnyAsync(t => t.Name == dto.Name &&
                                                              t.GameId == dto.GameId &&
                                                              t.AppUserId != userId);

            if (existedTeam) return new ServiceResult("Team already existed.");

            var team = _mapper.Map<Team>(dto);
            team.AppUserId = userId;

            await _uow.Teams.CreateOrUpdate(team);
            await _uow.CommitAsync();

            return new ServiceResult();
        }

        public async Task<ServiceResult> UpdateAsync(UpdateTeamDto dto, int userId)
        {
            var existedTeam = await _uow.Teams.AsQueryable()
                                              .FirstOrDefaultAsync(t => t.Id == dto.Id &&
                                                                        t.AppUserId == userId);

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
                                       .FirstOrDefaultAsync(t => t.Id == teamId &&
                                                                 t.AppUserId == userId);

            if (team == null) return new ServiceResult("Team not found.");

            await _uow.Teams.DeleteAsync(team.Id);
            await _uow.CommitAsync();

            return new ServiceResult();
        }

        public async Task<ServiceResult> RemoveFromTeamAsync(TeamActionDto dto)
        {
            var playerUserId = await _uow.Players.AsQueryable()
                                                 .Where(p => p.Id == dto.PlayerId &&
                                                             p.TeamId == dto.TeamId)
                                                 .Select(p => p.AppUserId)
                                                 .FirstOrDefaultAsync();

            if (playerUserId == 0) return new ServiceResult("Player not found.");

            var teamUserId = await _uow.Teams.AsQueryable()
                                             .Where(t => t.Id == dto.TeamId)
                                             .Select(t => t.AppUserId)
                                             .FirstOrDefaultAsync();

            if (teamUserId == 0) return new ServiceResult("Team not found.");

            if (playerUserId != dto.UserId || teamUserId != dto.UserId)
                return new ServiceResult("You do not have access to perform this operation.");

            var playerToRemove = await _uow.Players.AsQueryable()
                                                   .Where(p => p.Id == dto.PlayerId)
                                                   .FirstOrDefaultAsync();
            playerToRemove.TeamId = null;

            await _uow.Players.CreateOrUpdate(playerToRemove);
            await _uow.CommitAsync();

            return new ServiceResult();
        }

        public async Task<IList<PlayerListItemDto>> GetTeamPlayers(int teamId)
        {
            var players = await _uow.Teams.AsQueryable()
                                          .Where(t => t.Id == teamId)
                                          .Select(t => t.Players)
                                          .FirstOrDefaultAsync();

            return players == null
                ? null
                : _mapper.Map<IList<PlayerListItemDto>>(players);
        }

        public async Task<IList<TeamListItemDto>> GetListByUserIdAsync(int userId)
        {
            var listOfTeams = await _uow.Teams.AsQueryable()
                                              .Include(t => t.Game)
                                              .Include(t => t.Server)
                                              .Include(t => t.Goal)
                                              .Include(t => t.Invitations)
                                                .ThenInclude(i => i.Player)
                                              .Include(t => t.Invitations)
                                                .ThenInclude(i => i.Team)
                                              .Where(t => t.AppUserId == userId)
                                              .AsNoTracking()
                                              .ToListAsync();
            
            return _mapper.Map<IList<TeamListItemDto>>(listOfTeams);
        }

        public async Task<ServiceResult> UpdateLogoAsync(int userId, int teamId, string logo)
        {
            var team = await _uow.Teams.AsQueryable()
                                       .Where(t => t.AppUserId == userId && t.Id == teamId)
                                       .FirstOrDefaultAsync();

            if (team == null) return new ServiceResult("Team not found.");

            team.Photo = logo;

            await _uow.Teams.CreateOrUpdate(team);
            await _uow.CommitAsync();

            return new ServiceResult();
        }
    }
}
