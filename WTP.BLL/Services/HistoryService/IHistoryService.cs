using EntityFrameworkPaginateCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTP.BLL.DTOs.AppUserDTOs;
using WTP.BLL.DTOs.ServicesDTOs;
using WTP.BLL.Shared;
using WTP.DAL.Entities.AppUserEntities;

namespace WTP.BLL.Services.HistoryService
{
    public interface IHistoryService
    {
        Task CreateAsync(HistoryDto historyDto);
        Task UpdateAsync(HistoryDto historyDto);
        Task DeleteAsync(int id);
        Task<HistoryDto> GetAsync(int id);
        Task<IList<HistoryDto>> GetHistoryList();

        Task<IQueryable<History>> GetItemsOnPage(int page, int pageSize, IQueryable<History> baseQuery);
        Task<int> GetCountOfRecords();
        Task<IQueryable<History>> FilterByUserName(string name, IQueryable<History> baseQuery);
        Task<IQueryable<History>> SortByParam(HistorySortState sortOrder, IQueryable<History> baseQuery);
        Task<HistoryIndexDto> GetPageInfo(string name, int page, int pageSize,
            HistorySortState sortOrder);

        Task<Page<History>> GetFilteredSortedRecordsOnPage(int pageSize, int currentPage, string sortBy,
                                       string userName, string email, string operationName, string newUserName,
                                       string newUserEmail, int id, int adminId, bool sortOrder);
    }
}
