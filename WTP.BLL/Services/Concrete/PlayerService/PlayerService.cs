using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WTP.BLL.DTOs.AppUserDTOs;
using WTP.BLL.DTOs.PlayerDTOs;
using WTP.BLL.DTOs.ServicesDTOs;
using WTP.BLL.Services.Concrete.PlayerSrvices;
using WTP.BLL.Shared;
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
            catch(Exception ex)
            {
                // log error
                string s = ex.Message;
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

        public async Task<IList<PlayerListItemDto>> GetListByGameIdAsync(int gameId)
        {
            var listOfPlayers = await _uow.Players.GetListByGameIdAsync(gameId);

            return _mapper.Map<IList<PlayerListItemDto>>(listOfPlayers);
        }

        public async Task<IList<PlayerShortDto>> GetJoinedPlayersListAsync()
        {
            var allPlayers = await _uow.Players.AsQueryable().ToListAsync();
            return _mapper.Map<IList<PlayerShortDto>>(allPlayers);
        }

        public IQueryable<Player> FilterByName(string name, IQueryable<Player> baseQuery)
        {
            if (!String.IsNullOrEmpty(name))
                return baseQuery.Where(p => p.Name.Contains(name));

            return null;
        }

        public IQueryable<Player> SortByParam(PlayerSortState sortOrder, IQueryable<Player> baseQuery)
        {
            IQueryable<Player> query = Enumerable.Empty<Player>().AsQueryable();
            switch (sortOrder)
            {
                case PlayerSortState.NameDesc:
                    query = baseQuery.OrderByDescending(s => s.Name);
                    break;
                case PlayerSortState.NameAsc:
                    query = baseQuery.OrderBy(s => s.Name);
                    break;
                case PlayerSortState.EmailAsc:
                    query = baseQuery.OrderBy(s => s.AppUser.Email);
                    break;
                case PlayerSortState.EmailDesc:
                    query = baseQuery.OrderByDescending(s => s.AppUser.Email);
                    break;
                case PlayerSortState.IdAsc:
                    query = baseQuery.OrderBy(s => s.Id);
                    break;
                case PlayerSortState.IdDesc:
                    query = baseQuery.OrderByDescending(s => s.Id);
                    break;
                case PlayerSortState.UserIdAsc:
                    query = baseQuery.OrderBy(s => s.AppUserId);
                    break;
                case PlayerSortState.UserIdDesc:
                    query = baseQuery.OrderByDescending(s => s.AppUserId);
                    break;
                case PlayerSortState.UserNameAsc:
                    query = baseQuery.OrderBy(s => s.AppUser.UserName);
                    break;
                case PlayerSortState.UserNameDesc:
                    query = baseQuery.OrderByDescending(s => s.AppUser.UserName);
                    break;
                case PlayerSortState.TeamNameAsc:
                    query = baseQuery.OrderBy(s => s.Team.Name);
                    break;
                case PlayerSortState.TeamNameDesc:
                    query = baseQuery.OrderByDescending(s => s.Team.Name);
                    break;
                case PlayerSortState.GameNameAsc:
                    query = baseQuery.OrderBy(s => s.Game.Name);
                    break;
                case PlayerSortState.GameNameDesc:
                    query = baseQuery.OrderByDescending(s => s.Game.Name);
                    break;
                default:
                    query = baseQuery.OrderBy(s => s.Id);
                    break;
            }

            return query;
        }

        public IQueryable<Player> GetItemsOnPage(int page, int pageSize, IQueryable<Player> baseQuery)
        {
            IQueryable<Player> query = Enumerable.Empty<Player>().AsQueryable();
            query = baseQuery.Skip((page - 1) * pageSize).Take(pageSize);
            return query;
        }

        public async Task<int> GetCountOfPlayers()
        {
            return await _uow.Players.AsQueryable().CountAsync();
        }

        public async Task<PlayerManageDto> GetPageInfo(string name, int page, int pageSize,
            PlayerSortState sortOrder)
        {
            IQueryable<Player> query = _uow.Players.AsQueryable();
            IEnumerable<PlayerShortDto> items = Enumerable.Empty<PlayerShortDto>();

            try
            {
                var newQuery = FilterByName(name, query);
                newQuery = SortByParam(sortOrder, newQuery);
                newQuery = GetItemsOnPage(page, pageSize, newQuery);

                items = _mapper.Map<List<PlayerShortDto>>(newQuery.ToList());
            }
            catch (ArgumentNullException ex)
            {
                //TODO
                //_log.Error(ex.Message);
            }

            var count = await this.GetCountOfPlayers();

            PlayerManageDto viewModel = new PlayerManageDto
            {
                PageViewModel = new PageDto(count, page, pageSize),
                SortViewModel = new PlayerManageSortDto(sortOrder),
                Players = items
            };
            return viewModel;
        }
    }
}
