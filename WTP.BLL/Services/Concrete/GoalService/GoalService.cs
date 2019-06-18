using AutoMapper;
using EntityFrameworkPaginateCore;
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

        public async Task<GoalDto> GetByIdAsync(int goalId)
        {
            return _mapper.Map<GoalDto>(await _uow.Goals.GetByIdAsync(goalId));
        }

        public async Task<Page<Goal>> GetFilteredSortedGoalsOnPage(int pageSize, int currentPage, string sortBy
                                      , string name, int id, bool sortOrder)
        {
            Page<Goal> goals;
            var filters = new Filters<Goal>();
            filters.Add(!string.IsNullOrEmpty(name), x => x.Name.Contains(name));
            filters.Add(id != 0, x => x.Id.Equals(id));

            var sorts = new Sorts<Goal>();

            sorts.Add(sortBy == "Name", x => x.Name, sortOrder);
            sorts.Add(sortBy == "Id", x => x.Id, sortOrder);

            goals = await _uow.Goals.AsQueryable().PaginateAsync(currentPage, pageSize, sorts, filters);

            return goals;
        }
    }
}
