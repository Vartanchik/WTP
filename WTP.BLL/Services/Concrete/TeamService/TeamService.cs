using AutoMapper;
using EntityFrameworkPaginateCore;
using Microsoft.EntityFrameworkCore;
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

        public async Task<ServiceResult> CreateInvitationAsync(TeamActionDto dto)
        {
            var playerUserId = _uow.Players.AsQueryable()
                                           .Where(p => p.Id == dto.PlayerId)
                                           .Select(p => p.AppUserId)
                                           .FirstOrDefault();

            if (playerUserId == 0) return new ServiceResult("Player not found.");

            var teamCoachId = _uow.Teams.AsQueryable()
                                        .Where(t => t.Id == dto.TeamId)
                                        .Select(t => t.AppUserId)
                                        .FirstOrDefault();

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
                                              .AsNoTracking()
                                              .Where(p => p.AppUserId == userId)
                                              .ToListAsync();

            return _mapper.Map<IList<TeamListItemDto>>(listOfTeams);
        }

        public async Task<IList<InvitationListItemDto>> GetAllTeamInvitetionByUserId(int userId)
        {
            var listOfTeamId = await _uow.Teams.AsQueryable()
                                                 .Where(t => t.AppUserId == userId)
                                                 .Select(t => t.Id)
                                                 .ToListAsync();

            if (listOfTeamId == null) return null;

            var listOfInvitations = new List<Invitation>();

            foreach (var teamId in listOfTeamId)
            {
                var invitationsOfTeams = await _uow.Invitations.AsQueryable()
                                                               .Include(i => i.Player)
                                                               .Include(i => i.Team)
                                                               .Where(i => i.TeamId == teamId)
                                                               .ToListAsync();

                listOfInvitations.AddRange(invitationsOfTeams);
            }

            return _mapper.Map<List<InvitationListItemDto>>(listOfInvitations);
        }

        public async Task<Page<Team>> GetFilteredSortedTeamsOnPage(int pageSize, int currentPage, string sortBy
                                      , string name, int id, string game, int winRate, bool sortOrder)
        {
            Page<Team> teams;
            var filters = new Filters<Team>();
            filters.Add(!string.IsNullOrEmpty(name), x => x.Name.Contains(name));
            filters.Add(id != 0, x => x.Id.Equals(id));
            filters.Add(!string.IsNullOrEmpty(game), x => x.Game.Name.Contains(game));
            filters.Add(winRate != 0, x => x.WinRate.Equals(winRate));

            var sorts = new Sorts<Team>();

            sorts.Add(sortBy == "Name", x => x.Name, sortOrder);
            sorts.Add(sortBy == "Id", x => x.Id, sortOrder);
            sorts.Add(sortBy == "Game", x => x.Game, sortOrder);
            sorts.Add(sortBy == "WinRate", x => x.WinRate, sortOrder);

            teams = await _uow.Teams.AsQueryable().PaginateAsync(currentPage, pageSize, sorts, filters);

            return teams;
        }
    }
}
