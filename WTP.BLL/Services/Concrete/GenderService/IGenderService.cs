using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.BLL.Models.Gender;

namespace WTP.BLL.Services.Concrete.GenderService
{
    public interface IGenderService
    {
        Task CreateAsync(GenderModel genderDto);
        Task UpdateAsync(GenderModel genderDto);
        Task DeleteAsync(int id);
        Task<GenderModel> GetAsync(int id);
        Task<IEnumerable<GenderModel>> GetAllAsync();
    }
}
