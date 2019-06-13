using EntityFrameworkPaginateCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WTP.BLL.DTOs.PlayerDTOs;
using WTP.DAL.Entities;

namespace WTP.BLL.Services.Concrete.GoalService
{
    public interface IGoalService
    {
        Task<IList<GoalDto>> GetGoalsListAsync();
        Task CreateOrUpdateAsync(GoalDto dto, int? adminId = null);
        Task DeleteAsync(int goalId, int? adminId = null);
        Task<GoalDto> GetByIdAsync(int goalId);

        //For admin
        Task<Page<Goal>> GetFilteredSortedGoalsOnPage(int pageSize, int currentPage, string sortBy
                                      , string name, int id, bool sortOrder);
    }
}
