using System.Threading.Tasks;
using WTP.BLL.DTOs.TeamDTOs;

namespace WTP.BLL.Services.Concrete.TeamService
{
    public interface ITeamService
    {
        Task<ServiceResult> CreateAsync(CreateUpdateTeamDto dto, int userId);
        Task<ServiceResult> UpdateAsync(CreateUpdateTeamDto dto, int userId);
        Task<ServiceResult> DeleteAsync(int userId, int teamId);
        Task<ServiceResult> InviteToTeamAsync(int userId, int playerId);
        Task<ServiceResult> CancelInviteToTeamAsync(int userId, int playerId);
        Task<ServiceResult> AddToTeamAsync(int userId, int playerId, int teamId);
        Task<ServiceResult> RemoveFromTeamAsync(int userId, int playerId);
        Task<ServiceResult> ChangeTeamAsync(int userId, int playerId, int teamId);
    }
}
