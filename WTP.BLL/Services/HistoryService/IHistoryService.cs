using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WTP.BLL.DTOs.AppUserDTOs;
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

        IList<HistoryDto> FilterByUserName(List<HistoryDto> histories, string name);
        IList<HistoryDto> SortByParam(List<HistoryDto> histories, HistorySortState sortOrder);

    }
}
