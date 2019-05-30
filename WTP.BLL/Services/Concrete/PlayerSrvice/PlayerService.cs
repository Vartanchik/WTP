using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WTP.BLL.DTOs.AppUserDTOs;
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

        public async Task<ServiceResult> CreateOrUpdateAsync(CreateUpdatePlayerDto dto, int userId)
        {
            var bookedPlayer = _uow.Players.AsQueryable()
                .Where(p => p.Name == dto.Name && p.GameId == dto.GameId && p.AppUserId != userId)
                .FirstOrDefault();

            if (bookedPlayer != null)
            {
                return new ServiceResult("Player with such name already exists.");
            }

            try
            {
                var player = _uow.Players.AsQueryable()
                    .Where(p => p.AppUserId == userId && p.GameId == dto.GameId)
                    .FirstOrDefault();

                if (player == null)
                {
                    // create
                    player = _mapper.Map<Player>(dto);
                    player.AppUserId = userId;
                }
                else
                {
                    // update
                    player.About = dto.About;
                    player.Decency = dto.Decency;
                    player.GameId = dto.GameId;
                    player.GoalId = dto.GoalId;
                    player.Name = dto.Name;
                    player.RankId = dto.RankId;
                    player.ServerId = dto.ServerId;
                }

                await _uow.Players.CreateOrUpdate(player);
                await _uow.CommitAsync();

                return new ServiceResult();
            }
            catch
            {
                // log error
                return new ServiceResult("Server error.");
            }
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
            var listOfPlayers = await _uow.Players.GetListByUserIdAsync(userId);

            return _mapper.Map<IList<PlayerListItemDto>>(listOfPlayers);
        }

        public async Task<IList<PlayerListItemDto>> GetPlayersList()
        {
            List<PlayerListItemDto> result = new List<PlayerListItemDto>();

            var allPlayers = await _uow.Players.AsQueryable().ToListAsync();

            return _mapper.Map<IList<PlayerListItemDto>>(allPlayers);
        }

        public async Task<PlayerPaginationDto> GetListByGameIdWithSortAndFiltersAsync(int gameId, int page, int pageSize,
                                                                                      int idSort, int idSortType,
                                                                                      string nameValue,
                                                                                      int rankLeftValue, int rankRightValue,
                                                                                      int decencyLeftValue, int decencyRightValue)
        {
            IList<Player> listOfPlayers = null;

            switch (idSort)
            {
                    case 1:
                    {
                        if (idSortType == 1)
                        {
                            listOfPlayers = await _uow.Players.AsQueryable()
                                                    .Include(p => p.Game)
                                                    .Include(p => p.Server)
                                                    .Include(p => p.Goal)
                                                    .Include(p => p.Rank)
                                                    .OrderBy(s => s.Decency)
                                                    .Where(p => p.GameId == gameId 
                                                    && p.Name.Contains(nameValue)
                                                    && p.Rank.Value <= rankRightValue 
                                                    && p.Rank.Value >= rankLeftValue
                                                    && p.Decency <= decencyRightValue
                                                    && p.Decency >= decencyLeftValue)
                                                    .Skip((page - 1) * pageSize)
                                                    .Take(pageSize)
                                                    .ToListAsync();

                        }

                        else if (idSortType == 2)
                        {
                            listOfPlayers = await _uow.Players.AsQueryable()
                                                    .Include(p => p.Game)
                                                    .Include(p => p.Server)
                                                    .Include(p => p.Goal)
                                                    .Include(p => p.Rank)
                                                    .OrderByDescending(s => s.Decency)
                                                    .Where(p => p.GameId == gameId 
                                                    && p.Name.Contains(nameValue)
                                                    && p.Rank.Value <= rankRightValue
                                                    && p.Rank.Value >= rankLeftValue
                                                    && p.Decency <= decencyRightValue
                                                    && p.Decency >= decencyLeftValue)
                                                    .Skip((page - 1) * pageSize)
                                                    .Take(pageSize)
                                                    .ToListAsync();
                        }

                        else
                        {
                            listOfPlayers = await _uow.Players.AsQueryable()
                                                   .Include(p => p.Game)
                                                   .Include(p => p.Server)
                                                   .Include(p => p.Goal)
                                                   .Include(p => p.Rank)
                                                   .Where(p => p.GameId == gameId 
                                                   && p.Name.Contains(nameValue)
                                                   && p.Rank.Value <= rankRightValue
                                                   && p.Rank.Value >= rankLeftValue
                                                   && p.Decency <= decencyRightValue
                                                   && p.Decency >= decencyLeftValue)
                                                   .Skip((page - 1) * pageSize)
                                                   .Take(pageSize)
                                                   .ToListAsync();

                        }
                        break;
                    }

                    case 2:
                    {
                        if (idSortType == 1)
                        {
                            listOfPlayers = await _uow.Players.AsQueryable()
                                                    .Include(p => p.Game)
                                                    .Include(p => p.Server)
                                                    .Include(p => p.Goal)
                                                    .Include(p => p.Rank)
                                                    .OrderBy(s => s.Rank.Value)
                                                    .Where(p => p.GameId == gameId 
                                                    && p.Name.Contains(nameValue)
                                                    && p.Rank.Value <= rankRightValue
                                                    && p.Rank.Value >= rankLeftValue
                                                    && p.Decency <= decencyRightValue
                                                    && p.Decency >= decencyLeftValue)
                                                    .Skip((page - 1) * pageSize)
                                                    .Take(pageSize)
                                                    .ToListAsync();
                        }

                        else if (idSortType == 2)
                        {
                            listOfPlayers = await _uow.Players.AsQueryable()
                                                    .Include(p => p.Game)
                                                    .Include(p => p.Server)
                                                    .Include(p => p.Goal)
                                                    .Include(p => p.Rank)
                                                    .OrderByDescending(s => s.Rank.Value)
                                                    .Where(p => p.GameId == gameId 
                                                    && p.Name.Contains(nameValue)
                                                    && p.Rank.Value <= rankRightValue
                                                    && p.Rank.Value >= rankLeftValue
                                                    && p.Decency <= decencyRightValue
                                                    && p.Decency >= decencyLeftValue)
                                                    .Skip((page - 1) * pageSize)
                                                    .Take(pageSize)
                                                    .ToListAsync();
                        }

                        else
                        {
                            listOfPlayers = await _uow.Players.AsQueryable()
                                                   .Include(p => p.Game)
                                                   .Include(p => p.Server)
                                                   .Include(p => p.Goal)
                                                   .Include(p => p.Rank)
                                                   .Where(p => p.GameId == gameId
                                                   && p.Name.Contains(nameValue)
                                                   && p.Rank.Value <= rankRightValue
                                                   && p.Rank.Value >= rankLeftValue
                                                   && p.Decency <= decencyRightValue
                                                   && p.Decency >= decencyLeftValue)
                                                   .Skip((page - 1) * pageSize)
                                                   .Take(pageSize)
                                                   .ToListAsync();

                        }
                        break;
                    }
                        default:
                    {
                         listOfPlayers = await _uow.Players.AsQueryable()
                                                    .Include(p => p.Game)
                                                    .Include(p => p.Server)
                                                    .Include(p => p.Goal)
                                                    .Include(p => p.Rank)
                                                    .Where(p => p.GameId == gameId
                                                    && p.Name.Contains(nameValue)
                                                    && p.Rank.Value <= rankRightValue
                                                    && p.Rank.Value >= rankLeftValue
                                                    && p.Decency <= decencyRightValue
                                                    && p.Decency >= decencyLeftValue)
                                                    .Skip((page - 1) * pageSize)
                                                    .Take(pageSize)
                                                    .ToListAsync();
                         break;
                    }

            }

            var resultList = _mapper.Map<IList<PlayerListItemDto>>(listOfPlayers);

            int playersQuantity = _uow.Players.AsQueryable()
                                    .Where(p => p.GameId == gameId
                                    && p.Name.Contains(nameValue)
                                    && p.Rank.Value <= rankRightValue
                                    && p.Rank.Value >= rankLeftValue
                                    && p.Decency <= decencyRightValue
                                    && p.Decency >= decencyLeftValue)
                                    .Count();

            var resultModel = new PlayerPaginationDto
            {
                Players = resultList,
                PlayersQuantity = playersQuantity
            };

            return resultModel;
        }
    }
}
