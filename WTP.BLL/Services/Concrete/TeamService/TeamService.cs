using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<ServiceResult> CreateAsync(CreateUpdateTeamDto dto, int userId)
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

        public async Task<ServiceResult> UpdateAsync(CreateUpdateTeamDto dto, int userId)
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

        public Task<ServiceResult> InviteToTeamAsync(int userId, int playerId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> CancelInviteToTeamAsync(int userId, int playerId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> AddToTeamAsync(int userId, int playerId, int teamId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> RemoveFromTeamAsync(int userId, int playerId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> ChangeTeamAsync(int userId, int playerId, int teamId)
        {
            throw new NotImplementedException();
        }
    }
}
