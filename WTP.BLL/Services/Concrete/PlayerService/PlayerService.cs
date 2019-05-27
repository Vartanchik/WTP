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

        public async Task CreateOrUpdateAsync(PlayerDto dto, int adminId=1)
        {
            var player = _mapper.Map<Player>(dto);

            await _uow.Players.CreateOrUpdate(player);
            await _uow.CommitAsync();
        }

        public async Task DeleteAsync(int playerId, int adminId=1)
        {
            await _uow.Players.DeleteAsync(playerId);
            await _uow.CommitAsync();
        }

        public async Task<PlayerDto> FindAsync(int playerId)
        {
            var dto = _mapper.Map<PlayerDto>(await _uow.Players.GetAsync(playerId));

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

        public async Task<IQueryable<PlayerJoinedDto>> GetPlayersList()
        {
            var allPlayers = from player in await _uow.Players.AsQueryable().ToListAsync()
                join game in await _uow.Games.AsQueryable().ToListAsync() on player.GameId equals game.Id
                join team in await _uow.Teams.AsQueryable().ToListAsync() on player.TeamId equals team.Id
                join rank in await _uow.Ranks.AsQueryable().ToListAsync() on player.Game.Id equals rank.GameId
                join goal in await _uow.Goals.AsQueryable().ToListAsync() on player.GoalId equals goal.Id
                join server in await _uow.Servers.AsQueryable().ToListAsync() on player.ServerId equals server.Id
                join user in await _uow.AppUsers.AsQueryable().ToListAsync() on player.AppUserId equals user.Id
                    select _mapper.Map<PlayerJoinedDto>(player);

            return allPlayers.AsQueryable();//.ToList();
        }

        public async Task<IList<PlayerJoinedDto>> GetAllPlayersList()
        {
            var allPlayers = from user in await _uow.AppUsers.AsQueryable().ToListAsync()
            join player in await _uow.Players.AsQueryable().ToListAsync() on user.Id equals player.AppUserId
            join team in await _uow.Teams.AsQueryable().ToListAsync() on player.TeamId equals team.Id
                             join rank in await _uow.Ranks.AsQueryable().ToListAsync() on player.Rank.Id equals rank.GameId
                             join game in await _uow.Games.AsQueryable().ToListAsync() on rank.GameId/*player.GameId*/ equals game.Id
                             join server in await _uow.Servers.AsQueryable().ToListAsync() on player.ServerId equals server.Id
                             join goal in await _uow.Goals.AsQueryable().ToListAsync() on player.GoalId equals goal.Id
//                             group player by player.Id into res
                             select _mapper.Map<PlayerJoinedDto>(player);
            //var allPlayers = from player in await _uow.Players.AsQueryable().ToListAsync()
            //                 join user in await _uow.AppUsers.AsQueryable().ToListAsync() on  player.AppUserId/*user.Players.SingleOrDefault(p=>p.AppUserId==user.Id)*/ equals user.Id
            //                 join team in await _uow.Teams.AsQueryable().ToListAsync() on player.TeamId equals team.Id
            //                 join game in await _uow.Games.AsQueryable().ToListAsync() on player.GameId equals game.Id
            //                 join rank in await _uow.Ranks.AsQueryable().ToListAsync() on player.Game.Id equals rank.GameId
            //                 join goal in await _uow.Goals.AsQueryable().ToListAsync() on player.GoalId equals goal.Id
            //                 join server in await _uow.Servers.AsQueryable().ToListAsync() on player.ServerId equals server.Id
            //                 select _mapper.Map<PlayerJoinedDto>(player);

            return allPlayers.GroupBy(x => x.Id).Select(x => x.FirstOrDefault()).ToList();
        }

        public async Task<IList<PlayerDto>> GetPlayers()
        {
            var allPlayers = await _uow.Players.AsQueryable().Include(p=>p).Include(p=>p.Server).Include(p=>p.Rank).Include(p=>p.Team)
                .ToListAsync();

            return _mapper.Map<PlayerDto[]>(allPlayers);//.ToList();
        }

        public async Task<PlayerJoinedDto> GetPlayerInfo(int playerId)
        {
            var player = await GetAllPlayersList();
            return player.SingleOrDefault(p => p.Id == playerId);   
        }
    }
}
