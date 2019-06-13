using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WTP.BLL.DTOs.PlayerDTOs;
using WTP.BLL.DTOs.ServicesDTOs;
using WTP.DAL.Entities;
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
            var player = await _uow.Players.AsQueryable()
                                           .Include(p => p.AppUser)
                                              .ThenInclude(u => u.Country)
                                           .Include(p => p.AppUser)
                                              .ThenInclude(u => u.AppUserLanguages)
                                                 .ThenInclude(l => l.Language)
                                           .Include(p => p.Server)
                                           .Include(p => p.Goal)
                                           .Include(p => p.Rank)
                                           .Include(p => p.Team)
                                           .FirstOrDefaultAsync(p => p.Id == playerId);

            var dto = _mapper.Map<PlayerDto>(player);

            return dto;
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

        public async Task<ServiceResult> DeleteAsync(int userId, int gameId)
        {
            var player = _uow.Players.AsQueryable()
                                     .FirstOrDefault(p => p.AppUserId == userId &&
                                                          p.GameId == gameId);

            if (player == null) return new ServiceResult("Player not found.");

            await _uow.Players.DeleteAsync(player.Id);
            await _uow.CommitAsync();

            return new ServiceResult();
        }

        public async Task<IList<PlayerListItemDto>> GetListByUserIdAsync(int userId)
        {
            var listOfPlayers = await _uow.Players.AsQueryable()
                                                  .Include(p => p.Game)
                                                  .Include(p => p.Server)
                                                  .Include(p => p.Goal)
                                                  .Include(p => p.Rank)
                                                  .Include(p => p.Invitations)
                                                    .ThenInclude(i => i.Player)
                                                  .Include(p => p.Invitations)
                                                    .ThenInclude(i => i.Team)
                                                  .Where(p => p.AppUserId == userId)
                                                  .AsNoTracking()
                                                  .ToListAsync();

            // TODO: consider this variant of query
            //var listOfPlayers2 = await _uow.Players.AsQueryable()
            //                             .Where(p => p.AppUserId == userId)
            //                             .Select(p => new PlayerListItemDto
            //                             {
            //                                 Id = p.Id,
            //                                 Photo = p.AppUser.Photo,
            //                                 Name = p.Name,
            //                                 Game = p.Game.Name,
            //                                 Rank = p.Rank.Name,
            //                                 Server = p.Server.Name,
            //                                 Goal = p.Goal.Name,
            //                                 About = p.About,
            //                                 Decency = p.Decency,
            //                                 Invitations = p.Invitations.Select(i => new InvitationListItemDto
            //                                 {
            //                                     Id = i.Id,
            //                                     PlayerName = i.Player.Name,
            //                                     TeamName = i.Team.Name,
            //                                     Author = i.Author.ToString()
            //                                 }).ToList()
            //                             })
            //                             .AsNoTracking()
            //                             .ToListAsync();

            return _mapper.Map<IList<PlayerListItemDto>>(listOfPlayers);
        }

        public async Task<IList<PlayerListItemDto>> GetPlayersList()
        {
            var allPlayers = await _uow.Players.AsQueryable()
                                               .ToListAsync();

            var dto = _mapper.Map<IList<PlayerListItemDto>>(allPlayers);

            return dto;
        }

        public async Task<IList<PlayerListItemDto>> GetListByTeamIdAsync(int teamId)
        {
            var listOfPlayers = await _uow.Players.AsQueryable()
                                                  .Include(p => p.AppUser)
                                                  .Include(p => p.Game)
                                                  .Include(p => p.Server)
                                                  .Include(p => p.Goal)
                                                  .Include(p => p.Rank)
                                                  .AsNoTracking()
                                                  .Where(p => p.TeamId == teamId)
                                                  .ToListAsync();

            return _mapper.Map<IList<PlayerListItemDto>>(listOfPlayers);
        }

        public async Task<PlayerPaginationDto> GetFilteredPlayersByGameIdAsync(PlayerInputValuesModelDto inputValues)
        {
            IList<Player> listOfPlayers = null;

            //chose sort field 
            int? sortOperator(Player player)
            {
                switch(inputValues.SortField)
                {
                    case "decency":
                        return player.Decency;

                    case "rank":
                        return player.Rank.Value;

                    default:
                        return null;
                }
            }

            //add filter fields 
            bool filterOperator(Player player) {

                inputValues.NameValue = inputValues.NameValue == null ? "" : inputValues.NameValue;

                return player.GameId == inputValues.GameId
                       && player.Name.Contains(inputValues.NameValue)                       && player.Rank.Value <= inputValues.RankRightValue
                       && player.Rank.Value >= inputValues.RankLeftValue
                       && player.Decency <= inputValues.DecencyRightValue
                       && player.Decency >= inputValues.DecencyLeftValue;

            }
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
    }
}
