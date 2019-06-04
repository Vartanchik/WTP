using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WTP.BLL.DTOs.PlayerDTOs;
using WTP.BLL.DTOs.ServicesDTOs;
using WTP.BLL.DTOs.TeamDTOs;
using WTP.DAL.Entities;
using WTP.DAL.Entities.TeamEntities;
using WTP.DAL.UnitOfWork;

namespace WTP.BLL.Services.Concrete.PlayerSrvice
{
    public class PlayerService : IPlayerService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public PlayerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _uow = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PlayerDto> GetPlayerAsync(int playerId)
        {
            var player = await _uow.Players.GetByIdAsync(playerId);

            return _mapper.Map<PlayerDto>(player);
        }

        public async Task<ServiceResult> CreateAsync(CreatePlayerDto dto, int userId)
        {
            bool existedPlayer = _uow.Players.AsQueryable()
                                              .Any(p => p.Name == dto.Name &&
                                                        p.GameId == dto.GameId &&
                                                        p.AppUserId != userId);

            if (existedPlayer) return new ServiceResult("Player already existed.");

            var player = _mapper.Map<Player>(dto);
            player.AppUserId = userId;

            await _uow.Players.CreateOrUpdate(player);
            await _uow.CommitAsync();

            return new ServiceResult();
        }

        public async Task<ServiceResult> UpdateAsync(UpdatePlayerDto dto, int userId)
        {
            var existedPlayer = _uow.Players.AsQueryable()
                                            .FirstOrDefault(p => p.AppUserId == userId &&
                                                                 p.Id == dto.Id);

            if (existedPlayer == null) return new ServiceResult("Player not found.");

            existedPlayer.About = dto.About;
            existedPlayer.Decency = dto.Decency;
            existedPlayer.GoalId = dto.GoalId;
            existedPlayer.Name = dto.Name;
            existedPlayer.RankId = dto.RankId;
            existedPlayer.ServerId = dto.ServerId;

            await _uow.Players.CreateOrUpdate(existedPlayer);
            await _uow.CommitAsync();

            return new ServiceResult();
        }

        public async Task<ServiceResult> DeleteAsync(int userId, int playerGameId)
        {
            var player = _uow.Players.AsQueryable()
                .Where(p => p.AppUserId == userId && p.GameId == playerGameId)
                .FirstOrDefault();

            if (player != null)
            {
                await _uow.Players.DeleteAsync(player.Id);
                await _uow.CommitAsync();

                return new ServiceResult();
            }

            return new ServiceResult("Player not found.");
        }

        public async Task<PlayerDto> FindAsync(int playerId)
        {
            var dto = _mapper.Map<PlayerDto>(await _uow.Players.GetByIdAsync(playerId));

            return dto;
        }

        public IQueryable<CommentDto> FindCommentsAsync(int playerId)
        {
            return from c in _uow.Comments.AsQueryable()
                   select _mapper.Map<CommentDto>(c);
        }

        public IQueryable<MatchDto> FindMatchesAsync(int playerId)
        {
            return from m in _uow.Matches.AsQueryable()
                   select _mapper.Map<MatchDto>(m);
        }

        public async Task<IList<PlayerListItemDto>> GetListByUserIdAsync(int userId)
        {
            var listOfPlayers = await _uow.Players
                .AsQueryable()
                .Include(p => p.Game)
                .Include(p => p.Server)
                .Include(p => p.Goal)
                .Include(p => p.Rank)
                .AsNoTracking()
                .Where(p => p.AppUserId == userId)
                .ToListAsync();

            return _mapper.Map<IList<PlayerListItemDto>>(listOfPlayers);
        }

        public async Task<IList<PlayerListItemDto>> GetPlayersList()
        {
            List<PlayerListItemDto> result = new List<PlayerListItemDto>();

            var allPlayers = await _uow.Players.AsQueryable().ToListAsync();

            return _mapper.Map<IList<PlayerListItemDto>>(allPlayers);
        }

        public async Task<PlayerPaginationDto> GetFilteredPlayersByGameIdAsync(PlayerInputValuesModelDto inputValues)
        {
            IList<Player> listOfPlayers = null;

            //chose sort field 
            int? sortOperator(Player player)
            {
                if (inputValues.SortField == "decency")
                    return player.Decency;

                else if (inputValues.SortField == "rank")
                    return player.Rank.Value;

                else
                    return null;
            }

            //add filter fields 
            bool filterOperator(Player player) => player.GameId == inputValues.GameId
                                               && player.Name.Contains(inputValues.NameValue)
                                               && player.Rank.Value <= inputValues.RankRightValue
                                               && player.Rank.Value >= inputValues.RankLeftValue
                                               && player.Decency <= inputValues.DecencyRightValue
                                               && player.Decency >= inputValues.DecencyLeftValue;
            //sorting by ASC
            if (inputValues.SortType == "asc")
            {
                listOfPlayers = _uow.Players.AsQueryable()
                                            .Include(p => p.Game)
                                            .Include(p => p.Server)
                                            .Include(p => p.Goal)
                                            .Include(p => p.Rank)
                                            .OrderBy(sortOperator)
                                            .Where(filterOperator)
                                            .Skip((inputValues.Page - 1) * inputValues.PageSize)
                                            .Take(inputValues.PageSize)
                                            .ToList();
            }

            //sorting by DESC
            else if (inputValues.SortType == "desc")
            {
                listOfPlayers = _uow.Players.AsQueryable()
                                            .Include(p => p.Game)
                                            .Include(p => p.Server)
                                            .Include(p => p.Goal)
                                            .Include(p => p.Rank)
                                            .OrderByDescending(sortOperator)
                                            .Where(filterOperator)
                                            .Skip((inputValues.Page - 1) * inputValues.PageSize)
                                            .Take(inputValues.PageSize)
                                            .ToList();
            }

            //get filtered players without sorting
            else
            {
                listOfPlayers = _uow.Players.AsQueryable()
                                            .Include(p => p.Game)
                                            .Include(p => p.Server)
                                            .Include(p => p.Goal)
                                            .Include(p => p.Rank)
                                            .Where(filterOperator)
                                            .Skip((inputValues.Page - 1) * inputValues.PageSize)
                                            .Take(inputValues.PageSize)
                                            .ToList();
            }

            var mappedPlayersList = _mapper.Map<IList<PlayerListItemDto>>(listOfPlayers);

            //Count total quantity of filtered players 
            int playersQuantity = await _uow.Players.AsQueryable()
                                                    .Where(p => p.GameId == inputValues.GameId
                                                        && p.Name.Contains(inputValues.NameValue)
                                                        && p.Rank.Value <= inputValues.RankRightValue
                                                        && p.Rank.Value >= inputValues.RankLeftValue
                                                        && p.Decency <= inputValues.DecencyRightValue
                                                        && p.Decency >= inputValues.DecencyLeftValue)
                                                    .CountAsync();

            var resultModel = new PlayerPaginationDto
            {
                Players = mappedPlayersList,
                PlayersQuantity = playersQuantity
            };

            return resultModel;
        }

        public async Task<IList<PlayerListItemDto>> GetListByTeamIdAsync(int teamId)
        {
            var listOfPlayers = await _uow.Players.AsQueryable()
                                                  .Include(p => p.Game)
                                                  .Include(p => p.Server)
                                                  .Include(p => p.Goal)
                                                  .Include(p => p.Rank)
                                                  .AsNoTracking()
                                                  .Where(p => p.TeamId == teamId)
                                                  .ToListAsync();

            return _mapper.Map<IList<PlayerListItemDto>>(listOfPlayers);
        }

        public async Task<IList<InvitationListItemDto>> GetAllPlayerInvitetionByUserId(int userId)
        {
            var listOfPlayerId = await _uow.Players.AsQueryable()
                                                   .Where(p => p.AppUserId == userId)
                                                   .Select(p => p.Id)
                                                   .ToListAsync();

            if (listOfPlayerId == null) return null;

            var listOfInvitations = new List<Invitation>();

            foreach (var playerId in listOfPlayerId)
            {
                var invitationsOfPlayer = await _uow.Invitations.AsQueryable()
                                                                .Include(i => i.Player)
                                                                .Include(i => i.Team)
                                                                .Where(i => i.PlayerId == playerId)
                                                                .ToListAsync();

                listOfInvitations.AddRange(invitationsOfPlayer);
            }

            return _mapper.Map<List<InvitationListItemDto>>(listOfInvitations);
        }
    }
}
