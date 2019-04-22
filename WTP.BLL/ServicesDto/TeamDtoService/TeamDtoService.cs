using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WTP.BLL.ModelsDto;

namespace WTP.BLL.Services.TeamDtoService
{
    public class TeamDtoService : IMaintainableDto<TeamDto>
    {
        public Task<bool> CreateAsync(TeamDto teamDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<TeamDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TeamDto> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(TeamDto teamDto)
        {
            throw new NotImplementedException();
        }
    }
}
