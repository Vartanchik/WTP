using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.BLL.DTOs.TeamDTOs;

namespace WTP.BLL.Services.Concrete.TeamService
{
    public interface ITeamService
    {
        Task<IList<TeamDto>> GetTeamsListAsync();
        Task CreateOrUpdateAsync(TeamDto dto, int? adminId = null);
        Task DeleteAsync(int teamId, int? adminId = null);
        Task<TeamDto> FindAsync(int teamId);
    }
}
