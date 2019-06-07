using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.BLL.DTOs.PlayerDTOs;
using WTP.DAL.UnitOfWork;

namespace WTP.BLL.Services.Concrete.GameService
{
    public class GameService : IGameService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _ouw;

        public GameService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _ouw = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IList<GameDto>> GetAllGamesAsync()
        {
            var listOfGames = await _ouw.Games.AsQueryable().ToListAsync();

            return _mapper.Map<IList<GameDto>>(listOfGames);
        }
    }
}
