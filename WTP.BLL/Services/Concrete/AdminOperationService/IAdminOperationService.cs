using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WTP.BLL.ModelsDto.AdminOperation;
using WTP.DAL.DomainModels;

namespace WTP.BLL.Services.Concrete.AdminOperationService
{
    public interface IAdminOperationService
    {
        Task CreateAsync(AdminOperationDto adminOperationDto);
        Task UpdateAsync(AdminOperationDto adminOperationDto);
        Task DeleteAsync(int id);
        Task<AdminOperationDto> GetAsync(int id);
        Task<IEnumerable<AdminOperationDto>> GetAllAsync();
    }
}
