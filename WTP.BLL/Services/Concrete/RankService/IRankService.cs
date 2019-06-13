using EntityFrameworkPaginateCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WTP.BLL.DTOs.PlayerDTOs;
using WTP.DAL.Entities;

namespace WTP.BLL.Services.Concrete.RankService
{
    public interface IRankService
    {
        Task<IList<RankDto>> GetRanksListAsync();
        Task CreateOrUpdateAsync(RankDto dto, int? adminId = null);
        Task DeleteAsync(int rankId, int? adminId = null);
        Task<RankDto> GetByIdAsync(int rankId);

        //For Admin
        Task<Page<Rank>> GetFilteredSortedRanksOnPage(int pageSize, int currentPage, string sortBy
                                      , string name, int id, int value, bool sortOrder);
    }
}
