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

namespace WTP.BLL.Services.Concrete.RankService
{
    public class RankService : IRankService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        public RankService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _uow = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IList<RankDto>> GetRanksListAsync()
        {
            var listOfranks = from rank in await _uow.Ranks.AsQueryable().ToListAsync()
                             //   join game in await _uow.Games.AsQueryable().ToListAsync() on rank.Id equals game.Id
                             select _mapper.Map<RankDto>(rank);

            return listOfranks.ToList();
            //return _mapper.Map<IList<RankDto>>(listOfranks);
        }

        public async Task CreateOrUpdateAsync(RankDto dto, int? adminId = null)
        {
            var rank = _mapper.Map<Rank>(dto);

            await _uow.Ranks.CreateOrUpdate(rank);
            await _uow.CommitAsync();
        }

        public async Task DeleteAsync(int rankId, int? adminId = null)
        {
            await _uow.Ranks.DeleteAsync(rankId);
            await _uow.CommitAsync();
        }

        public async Task<RankDto> GetByIdAsync(int rankId)
        {
            return _mapper.Map<RankDto>(await _uow.Ranks.GetByIdAsync(rankId));
        }
    }
}
