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

            if (existed) return new ServiceResult("Team already existed");

            var team = _mapper.Map<Team>(dto);
            team.CoachId = userId;

            await _uow.Teams.CreateOrUpdate(team);
            await _uow.CommitAsync();

            return new ServiceResult(true);
        }

        public async Task<ServiceResult> UpdateAsync(CreateUpdateTeamDto dto, int userId)
        {
            var existedTeam = _uow.Teams.AsQueryable()
                                        .FirstOrDefault(t => t.CoachId == userId &&
                                                           t.GameId == dto.GameId);

            if (existedTeam == null) return new ServiceResult("Team not found");

            existedTeam.GoalId = dto.GoalId;
            existedTeam.Name = dto.Name;
            existedTeam.ServerId = dto.ServerId;

            await _uow.Teams.CreateOrUpdate(existedTeam);
            await _uow.CommitAsync();

            return new ServiceResult(true);
        }

        public async Task<ServiceResult> DeleteAsync(int userId, int teamId)
        {
            var team = _uow.Teams.AsQueryable()
                .FirstOrDefault(t => t.CoachId == userId && t.Id == teamId);

            if (team == null)
            {
                return new ServiceResult("Player not found.");
            }

            await _uow.Teams.DeleteAsync(team.Id);
            await _uow.CommitAsync();

            return new ServiceResult();
        }

        // not finished!
        //public async Task<ServiceResult> InviteAsync(int userId, int playerId)
        //{
        //    var player = await _uow.Players.GetByIdAsync(playerId);
        //    if (player == null)
        //    {
        //        return new ServiceResult("User not found.");
        //    }

        //    var team = _uow.Teams.AsQueryable().Where(t => t.CoachId == userId).FirstOrDefault();
        //    if (team == null)
        //    {
        //        return new ServiceResult("Team not found.");
        //    }
        //    if (team.GameId != player.GameId)
        //    {
        //        return new ServiceResult("The player must be from the same game as team.");
        //    }

        //    // add without invite:
        //    // if (team.CoachId == userId) + if (team.Players.Count() < 5) ... add to team

        //    var invite = _mapper.Map<Invite>(new InviteDto(player.Id, team.Id, true));
        //    await _uow.Invites.CreateOrUpdate(invite);

        //    return new ServiceResult();
        //}

        // not valid !
        public async Task<ServiceResult> AddToTeamAsync(int playerId, int teamId, int userId)
        {
            var team = await _uow.Teams.AsQueryable().Include(t => t.Players).FirstOrDefaultAsync(t => t.Id == teamId && t.CoachId == userId);

            if (team == null) return new ServiceResult();




            var coach = await _uow.AppUsers.GetByIdAsync(userId);
            if (coach == null)
            {
                return new ServiceResult("User not found.");
            }

            var team = coach.Teams.AsQueryable().Where(t => t.GameId == dto.GameId).FirstOrDefault();
            if (team == null)
            {
                return new ServiceResult("The user does not have a team in the selected game.");
            }
            if (team.Players.Count() >= 5)
            {
                return new ServiceResult("The team is full.");
            }

            var player = await _uow.Players.GetByIdAsync(dto.PlayerId);
            if (player == null)
            {
                return new ServiceResult("Player not found.");
            }
            if (player.GameId != dto.GameId)
            {
                return new ServiceResult("The player must be from the same game as team.");
            }
            if (team.Players.Contains(player))
            {
                return new ServiceResult("The player has already been added to the team.");
            }

            team.Players.Add(player);

            await _uow.Teams.CreateOrUpdate(team);
            await _uow.CommitAsync();

            return new ServiceResult();
        }
    }
}
