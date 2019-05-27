using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WTP.BLL.DTOs.PlayerDTOs;
using WTP.DAL.Entities;
using WTP.DAL.UnitOfWork;

namespace WTP.BLL.Services.Concrete.GoalService
{
    public class GoalService : IGoalService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        public GoalService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _uow = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IList<GoalDto>> GetGoalsListAsync()
        {
            var listOfgolas = await _uow.Goals.AsQueryable().ToListAsync();
            return _mapper.Map<IList<GoalDto>>(listOfgolas);
        }

        public async Task CreateOrUpdateAsync(GoalDto dto, int? adminId = null)
        {
            var goal = _mapper.Map<Goal>(dto);

            await _uow.Goals.CreateOrUpdate(goal);
            await _uow.CommitAsync();
        }

        public async Task DeleteAsync(int goalId, int? adminId = null)
        {
            await _uow.Goals.DeleteAsync(goalId);
            await _uow.CommitAsync();
        }

        public async Task<GoalDto> FindAsync(int goalId)
        {
            return _mapper.Map<GoalDto>(await _uow.Goals.GetAsync(goalId));
        }
    }
}
