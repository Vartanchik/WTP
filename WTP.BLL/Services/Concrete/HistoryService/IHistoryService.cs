using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WTP.BLL.ModelsDto.History;

namespace WTP.BLL.Services.Concrete.HistoryService
{
    public interface IHistoryService
    {
        Task CreateAsync(HistoryDto historyDto);
        Task UpdateAsync(HistoryDto historyDto);
        Task DeleteAsync(int id);
        Task<HistoryDto> GetAsync(int id);
        Task<IEnumerable<HistoryDto>> GetAllAsync();
    }
}
