using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WTP.BLL.DTOs.PlayerDTOs;
using WTP.BLL.DTOs.TeamDTOs;
using WTP.DAL;
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

        public async Task<ServiceResult> UpdateAsync(UpdateTeamDto dto, int userId)
        {
            var existedTeam = _uow.Teams.AsQueryable()
                                        .FirstOrDefault(t => t.Id == dto.Id &&
                                                             t.CoachId == userId);

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
                                                                 t.CoachId == userId);

            if (team == null) return new ServiceResult("Team not found.");

            await _uow.Teams.DeleteAsync(team.Id);
            await _uow.CommitAsync();

            return new ServiceResult();
        }

        public IList<PlayerListItemDto> GetTeamPlayers(int teamId)
        {
            var players = _uow.Teams.AsQueryable()
                                    .FirstOrDefault(t => t.Id == teamId)
                                    .Players;

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
                                              .Where(p => p.CoachId == userId)
                                              .ToListAsync();

            return _mapper.Map<IList<TeamListItemDto>>(listOfTeams);
        }

        public async Task<ServiceResult> UpdateLogoAsync(int userId, int teamId, string logo)
        {
            var team = await _uow.Teams.AsQueryable()
                                       .Where(t => t.CoachId == userId && t.Id == teamId)
                                       .FirstOrDefaultAsync();

            if (team == null) return new ServiceResult("Team not found.");

            team.Photo = logo;

            await _uow.Teams.CreateOrUpdate(team);
            await _uow.CommitAsync();

            return new ServiceResult();
        }

        public async Task<ServiceResult> DeclineInvitationAsync(InviteActionDto dto)
        {
            var invitation = await _uow.Invitations.GetByIdAsync(dto.InvitationId);

            if (invitation == null) return new ServiceResult("Invitation not found.");

            if (!(dto.UserId == invitation.PlayerId && invitation.Author == Author.Coach ||
                dto.UserId == invitation.TeamId && invitation.Author == Author.Player))
            {
                return new ServiceResult("Error access.");
            }

            await _uow.Invitations.DeleteAsync(invitation.Id);
            await _uow.CommitAsync();

            return new ServiceResult();
        }

        private async Task<ServiceResult> AddToTeamAsync(int playerId, int teamId)
        {
            var team = await _uow.Teams.GetByIdAsync(teamId);

            if (team == null) return new ServiceResult("Team not found.");

            if (team.Players.Count >= 5) return new ServiceResult("Team is fuel.");

            var player = await _uow.Players.GetByIdAsync(playerId);

            if (player == null) return new ServiceResult("Player not found.");

            if (player.TeamId.GetValueOrDefault() != 0)
                return new ServiceResult("In order to state part of the team you must exit the current one.");

            player.Team = team;

            await _uow.Players.CreateOrUpdate(player);
            await _uow.CommitAsync();

            return new ServiceResult();
        }

        public async Task<ServiceResult> RemoveFromTeamAsync(TeamActionDto dto)
        {
            var existedPlayer = await _uow.Players.AsQueryable()
                                                  .AnyAsync(p => p.Id == dto.PlayerId &&
                                                                 p.TeamId == dto.TeamId);

            if (!existedPlayer) return new ServiceResult("Player not found.");

            // info about access to delete
            var existedTeam = await _uow.Teams.AsQueryable()
                                              .AnyAsync(t => t.Id == dto.TeamId &&
                                                             t.CoachId == dto.UserId);

            if (!existedTeam) return new ServiceResult("Team not found.");

            var playerToModify = await _uow.Players.AsQueryable()
                                                   .Where(p => p.Id == dto.PlayerId)
                                                   .FirstOrDefaultAsync();
            playerToModify.TeamId = null;
            // ?
            // @Vardan CreateOrUpdate ? 
            await _uow.CommitAsync();

            return new ServiceResult();

        }

        public async Task<IList<InvitationListItemDto>> GetAllTeamInvitetionByUserId(int userId)
        {
            var listOfTeamId = await _uow.Teams.AsQueryable()
                                                 .Where(t => t.CoachId == userId)
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

        public async Task<ServiceResult> CreateInvitationAsync(TeamActionDto dto)
        {
            // !
            // same query to diff repositories:
            var playerUserId = _uow.Players.AsQueryable()
                                           .Where(p => p.Id == dto.PlayerId)
                                           .Select(p => p.AppUserId)
                                           .FirstOrDefault();

            if (playerUserId == 0) return new ServiceResult("Player not found.");

            var teamCoachId = _uow.Teams.AsQueryable()
                                           .Where(t => t.Id == dto.PlayerId)
                                           .Select(t => t.CoachId)
                                           .FirstOrDefault();

            if (teamCoachId == 0) return new ServiceResult("Team not found.");

            Author author;

            if (playerUserId == dto.UserId) author = Author.Player;
            else if (teamCoachId == dto.UserId) author = Author.Coach;
            else return new ServiceResult("No access.");

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

        public async Task<ServiceResult> AcceptInvitationAsync(InviteActionDto dto)
        {
            var invitation = await _uow.Invitations.GetByIdAsync(dto.InvitationId);

            if (invitation == null) return new ServiceResult("Invitation not found.");

            // !
            // same query to diff repositories:
            var playerUserId = _uow.Players.AsQueryable()
                                           .Select(p => p.AppUserId)
                                           .FirstOrDefault(id => id == dto.UserId);

            if (playerUserId == 0) return new ServiceResult("Player not found.");

            var teamCoachId = _uow.Teams.AsQueryable()
                                        .Select(t => t.CoachId)
                                        .FirstOrDefault(id => id == dto.UserId);

            if (teamCoachId == 0) return new ServiceResult("Team not found.");

            if (dto.UserId == playerUserId && invitation.Author == Author.Coach ||
                dto.UserId == teamCoachId && invitation.Author == Author.Player)
            {
                return await AddToTeamAsync(invitation.PlayerId, invitation.TeamId);
            }
            else
            {
                return new ServiceResult("Error access.");
            }
        }

        /*
         * create IOwner interface { int AppUserId }
         * 
        private async int GetOwnerId(IQueryable<IEntity> entities)
        {
            return entities.Select(x => x.OWNER_ID)
        }
        */

        // FirstOrDefauldAsync
    }
}
