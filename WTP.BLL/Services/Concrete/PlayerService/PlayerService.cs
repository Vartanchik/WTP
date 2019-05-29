using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WTP.BLL.DTOs.AppUserDTOs;
using WTP.BLL.DTOs.PlayerDTOs;
using WTP.BLL.DTOs.ServicesDTOs;
using WTP.BLL.Services.Concrete.PlayerSrvices;
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

        public async Task<IList<PlayerListItemDto>> GetListByGameIdAsync(int gameId)
        {
            var listOfPlayers = await _uow.Players.GetListByGameIdAsync(gameId);

            return _mapper.Map<IList<PlayerListItemDto>>(listOfPlayers);
        }

        //public async Task<IQueryable<PlayerJoinedDto>> GetPlayersList()
        //{
        //    var allPlayers = from player in await _uow.Players.AsQueryable().ToListAsync()
        //                     join game in await _uow.Games.AsQueryable().ToListAsync() on player.GameId equals game.Id
        //                     join team in await _uow.Teams.AsQueryable().ToListAsync() on player.TeamId equals team.Id
        //                     join rank in await _uow.Ranks.AsQueryable().ToListAsync() on player.RankId equals rank.Id
        //                     join goal in await _uow.Goals.AsQueryable().ToListAsync() on player.GoalId equals goal.Id
        //                     join server in await _uow.Servers.AsQueryable().ToListAsync() on player.ServerId equals server.Id
        //                     join user in await _uow.AppUsers.AsQueryable().ToListAsync() on player.AppUserId equals user.Id
        //                     select _mapper.Map<PlayerJoinedDto>(player);

        //    return allPlayers.AsQueryable();
        //}

        public async Task<IList<PlayerJoinedDto>> GetAllPlayersList()
        {
            var allPlayers = from user in await _uow.AppUsers.AsQueryable().ToListAsync()
                             join player in await _uow.Players.AsQueryable().ToListAsync() on user.Id equals player.AppUserId
                             join team in await _uow.Teams.AsQueryable().ToListAsync() on player.TeamId equals team.Id
                             join rank in await _uow.Ranks.AsQueryable().ToListAsync() on player.RankId equals rank.Id
                             join game in await _uow.Games.AsQueryable().ToListAsync() on player.GameId/*player.GameId*/ equals game.Id
                             join server in await _uow.Servers.AsQueryable().ToListAsync() on player.ServerId equals server.Id
                             join goal in await _uow.Goals.AsQueryable().ToListAsync() on player.GoalId equals goal.Id
                             select _mapper.Map<PlayerJoinedDto>(player);

            return allPlayers.GroupBy(x => x.Id).Select(x => x.FirstOrDefault()).ToList();
        }

        public async Task<IList<PlayerDto>> GetPlayers()
        {
            var allPlayers = await _uow.Players.AsQueryable().ToListAsync();
            return _mapper.Map<IList<PlayerDto>>(allPlayers);
        }

        public async Task<PlayerJoinedDto> GetPlayerInfo(int playerId)
        {
            var player = await GetAllPlayersList();
            return player.SingleOrDefault(p => p.Id == playerId);
        }

        public Task<IQueryable<PlayerJoinedDto>> GetJoinedPlayersList()
        {
            throw new System.NotImplementedException();
        }
    }
}
