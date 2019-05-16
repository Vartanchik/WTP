using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WTP.BLL.ModelsDto.History;
using WTP.BLL.Shared.HistoryState;

namespace WTP.BLL.Services.Concrete.HistoryService
{
    public interface IHistoryService
    {
        Task CreateAsync(HistoryDto historyDto);
        Task UpdateAsync(HistoryDto historyDto);
        Task DeleteAsync(int id);
        Task<HistoryDto> GetAsync(int id);
        Task<IEnumerable<HistoryDto>> GetAllAsync();

        List<HistoryDto> Filter(List<HistoryDto> histories, string name);
        List<HistoryDto> Sort(List<HistoryDto> histories, HistorySortState sortOrder);
    }
}
