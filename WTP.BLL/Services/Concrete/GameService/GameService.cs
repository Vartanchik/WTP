using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public IEnumerable<GameDto> GetAllGames()
        {
            var listOfGames =  _ouw.Games.AsQueryable().ToList();

            return _mapper.Map<IEnumerable<GameDto>>(listOfGames);
        }
    }
}
