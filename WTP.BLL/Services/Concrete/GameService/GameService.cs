using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTP.BLL.DTOs.PlayerDTOs;
using WTP.DAL.Entities;
using WTP.DAL.UnitOfWork;

namespace WTP.BLL.Services.Concrete.GameService
{
    public class GameService : IGameService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        public GameService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _uow = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<GameDto> GetAllGames()
        {
            var listOfGames =  _uow.Games.AsQueryable().ToList();

            return _mapper.Map<IEnumerable<GameDto>>(listOfGames);
        }

        public async Task<IList<GameDto>> GetGamesListAsync()
        {
            var listOfGames = await _uow.Games.AsQueryable().ToListAsync();
            return _mapper.Map<IList<GameDto>>(listOfGames);
        }

        public async Task CreateOrUpdateAsync(GameDto dto, int? adminId = null)
        {
            var game = _mapper.Map<Game>(dto);

            await _uow.Games.CreateOrUpdate(game);
            await _uow.CommitAsync();
        }

        public async Task DeleteAsync(int gameId, int? adminId = null)
        {
            await _uow.Games.DeleteAsync(gameId);
            await _uow.CommitAsync();
        }

        public async Task<GameDto> FindAsync(int gameId)
        {
            return _mapper.Map<GameDto>(await _uow.Games.GetAsync(gameId));
        }
    }
}
