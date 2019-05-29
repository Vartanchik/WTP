using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.BLL.DTOs.TeamDTOs;

namespace WTP.BLL.Services.Concrete.TeamService
{
    public interface ITeamService
    {
        Task<ServiceResult> CreateOrUpdateAsync(CreateUpdateTeamDto dto, int userId);
        Task<ServiceResult> DeleteAsync(int userId, int gameId);
        Task<ServiceResult> AddToTeamAsync(AddPlayerToTeamDto dto, int userId);
    }
}
