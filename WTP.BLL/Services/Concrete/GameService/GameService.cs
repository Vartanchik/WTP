using AutoMapper;
using EntityFrameworkPaginateCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

        public async Task<IList<GameDto>> GetAllGamesAsync()
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
            return _mapper.Map<GameDto>(await _uow.Games.GetByIdAsync(gameId));
        }

        public async Task<Page<Game>> GetFilteredSortedGamesOnPage(int pageSize, int currentPage, string sortBy
                                      , string name, int id, bool sortOrder)
        {
            Page<Game> games;
            var filters = new Filters<Game>();
            filters.Add(!string.IsNullOrEmpty(name), x => x.Name.Contains(name));
            filters.Add(id!=0, x => x.Id.Equals(id));

            var sorts = new Sorts<Game>();

            sorts.Add(sortBy == "Name", x => x.Name, sortOrder);
            sorts.Add(sortBy == "Id", x => x.Id, sortOrder);

            games = await _uow.Games.AsQueryable().PaginateAsync(currentPage, pageSize, sorts, filters);

            return games;
        }
    }
}
