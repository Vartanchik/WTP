using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WTP.BLL.ModelsDto;

namespace WTP.BLL.Services.PlayerDtoService
{
    public class PlayerDtoService : IMaintainableDto<PlayerDto>
    {
        public Task<bool> CreateAsync(PlayerDto playerDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<PlayerDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<PlayerDto> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(PlayerDto playerDto)
        {
            throw new NotImplementedException();
        }
    }
}
