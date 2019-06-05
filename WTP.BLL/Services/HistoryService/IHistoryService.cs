using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WTP.BLL.DTOs.AppUserDTOs;
using WTP.BLL.DTOs.ServicesDTOs;
using WTP.BLL.Shared;

namespace WTP.BLL.Services.HistoryService
{
    public interface IHistoryService
    {
        Task CreateAsync(HistoryDto historyDto);
        Task UpdateAsync(HistoryDto historyDto);
        Task DeleteAsync(int id);
        Task<HistoryDto> GetAsync(int id);
        Task<IList<HistoryDto>> GetHistoryList();

        Task<List<HistoryDto>> GetItemsOnPage(int page, int pageSize);
        Task<int> GetCountOfRecords();
        IList<HistoryDto> FilterByUserName(List<HistoryDto> histories, string name);
        IList<HistoryDto> SortByParam(List<HistoryDto> histories, HistorySortState sortOrder);
        Task<HistoryIndexDto> GetPageInfo(string name, int page, int pageSize,
            HistorySortState sortOrder);

    }
}
