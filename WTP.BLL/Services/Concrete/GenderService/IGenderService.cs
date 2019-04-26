using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.BLL.ModelsDto.Gender;

namespace WTP.BLL.Services.Concrete.GenderService
{
    public interface IGenderService
    {
        Task CreateAsync(GenderDto genderDto);
        Task UpdateAsync(GenderDto genderDto);
        Task DeleteAsync(int id);
        Task<GenderDto> GetAsync(int id);
        Task<IEnumerable<GenderDto>> GetAllAsync();
    }
}
