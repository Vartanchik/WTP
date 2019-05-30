using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WTP.BLL.DTOs.TeamDTOs;
using WTP.DAL.Entities;
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

        public async Task<ServiceResult> CreateOrUpdateAsync(CreateUpdateTeamDto dto, int userId)
        {
            var boockedTeam = _uow.Teams.AsQueryable()
                .Where(t => t.Name == dto.Name && t.GameId == dto.GameId && t.CoachId != userId)
                .FirstOrDefault();

            if (boockedTeam != null)
            {
                return new ServiceResult("Team with such name already exists.");
            }

            try
            {
                var team = _uow.Teams.AsQueryable()
                    .Where(t => t.CoachId == userId && t.GameId == dto.GameId)
                    .FirstOrDefault();

                if (team == null)
                {
                    // create
                    team = _mapper.Map<Team>(dto);
                    team.CoachId = userId;
                }
                else
                {
                    // update
                    team.GoalId = dto.GoalId;
                    team.Name = dto.Name;
                    team.ServerId = dto.ServerId;
                }

                await _uow.Teams.CreateOrUpdate(team);
                await _uow.CommitAsync();

                return new ServiceResult();
            }
            catch(Exception ex)
            {
                string exe = ex.Message;
                // error logging
                return new ServiceResult("Server error.");
            }
        }

        public async Task<ServiceResult> DeleteAsync(int userId, int gameId)
        {
            var team = _uow.Teams.AsQueryable()
                .Where(t => t.CoachId == userId && t.GameId == gameId)
                .FirstOrDefault();

            if (team == null)
            {
                return new ServiceResult("Player not found.");
            }

            try
            {
                // delete
                await _uow.Players.DeleteAsync(team.Id);
                await _uow.CommitAsync();

                return new ServiceResult();
            }
            catch
            {
                // error logging
                return new ServiceResult("Server error.");
            }
        }

        public async Task<ServiceResult> AddToTeamAsync(AddPlayerToTeamDto dto, int userId)
        {
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

        public async Task<IList<TeamListItemDto>> GetListByUserIdAsync(int userId)
        {
            var listOfTeams = await _uow.Teams
                .AsQueryable()
                .Include(t => t.Game)
                .Include(t => t.Server)
                .Include(t => t.Goal)
                .AsNoTracking()
                .Where(p => p.CoachId == userId)
                .ToListAsync();

            return _mapper.Map<IList<TeamListItemDto>>(listOfTeams);
        }

        public async Task<ServiceResult> UpdateLogoAsync(int userId, int gameId, string logo)
        {
            var team = await _uow.Teams
                .AsQueryable()
                .Where(t => t.CoachId == userId && t.GameId == gameId)
                .FirstOrDefaultAsync();

            if (team == null)
            {
                return new ServiceResult("Team not found.");
            }

            team.Photo = logo;

            await _uow.Teams.CreateOrUpdate(team);
            await _uow.CommitAsync();

            return new ServiceResult();
        }

    }
}
