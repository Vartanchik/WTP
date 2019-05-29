using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WTP.BLL.DTOs.PlayerDTOs;

namespace WTP.BLL.Services.Concrete.RankService
{
    public interface IRankService
    {
        Task<IList<RankDto>> GetRanksListAsync();
        Task CreateOrUpdateAsync(RankDto dto, int? adminId = null);
        Task DeleteAsync(int rankId, int? adminId = null);
        Task<RankDto> FindAsync(int rankId);
    }
}
