using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WTP.BLL.ModelsDto.Operation;

namespace WTP.BLL.Services.Concrete.OperationService
{
    public interface IOperationService
    {
        Task CreateAsync(OperationDto operationDto);
        Task UpdateAsync(OperationDto operationDto);
        Task DeleteAsync(int id);
        Task<OperationDto> GetAsync(int id);
        Task<IEnumerable<OperationDto>> GetAllAsync();
    }
}
