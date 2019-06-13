using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WTP.BLL.DTOs.PlayerDTOs;

namespace WTP.BLL.Services.Concrete.GoalService
{
    public interface IGoalService
    {
        Task<IList<GoalDto>> GetGoalsListAsync();
        Task CreateOrUpdateAsync(GoalDto dto, int? adminId = null);
        Task DeleteAsync(int goalId, int? adminId = null);
        Task<GoalDto> GetByIdAsync(int goalId);
    }
}
