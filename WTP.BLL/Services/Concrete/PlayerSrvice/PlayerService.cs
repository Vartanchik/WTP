using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WTP.BLL.DTOs.AppUserDTOs;
using WTP.BLL.DTOs.PlayerDTOs;
using WTP.DAL.Entities;
using WTP.DAL.UnitOfWork;

namespace WTP.BLL.Services.Concrete.PlayerSrvice
{
    public class PlayerService : IPlayerService
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        
        public PlayerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _uof = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateOrUpdateAsync(PlayerDto dto)
        {
            var player = _mapper.Map<Player>(dto);

            await _uof.Players.CreateOrUpdate(player);
        }

        public async Task DeleteAsync(int playerId)
        {
            await _uof.Players.DeleteAsync(playerId);
        }

        public async Task<PlayerDto> FindAsync(int playerId)
        {
            var dto = _mapper.Map<PlayerDto>(await _uof.Players.GetByIdAsync(playerId));

            return dto;
        }

        public IQueryable<CommentDto> FindCommentsAsync(int playerId)
        {
            return from c in _uof.Comments.AsQueryable()
                   select _mapper.Map<CommentDto>(c);
        }

        public IQueryable<MatchDto> FindMatchesAsync(int playerId)
        {
            return from m in _uof.Matches.AsQueryable()
                   select _mapper.Map<MatchDto>(m);
        }

        public IList<PlayerDto> GetPlayersByUserId(int userId)
        {
            var listOfPlayers = _uof.Players.GetPlayersByUserId(userId);

            return  _mapper.Map<IList<PlayerDto>>(listOfPlayers);
        }
    }
}
