using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
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
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public PlayerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _uow = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateOrUpdateAsync(PlayerDto dto)
        {
            var player = _mapper.Map<Player>(dto);

            await _uow.Players.CreateOrUpdate(player);
        }

        public async Task DeleteAsync(int userId, int playerId)
        {
            var existedUser = await _uow.Players.AsQueryable().SingleOrDefaultAsync(p => p.Id == playerId);

            if (userId != existedUser.Id) throw new AccessViolationException("Operation failed");

            _uow.Players.Delete(existedUser);

            await _uow.CommitAsync();
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
            var listOfPlayers = await _uow.Players.AsQueryable().Where(p => p.AppUserId == userId).ToListAsync();

            return _mapper.Map<IList<PlayerListItemDto>>(listOfPlayers);
        }
    }
}
