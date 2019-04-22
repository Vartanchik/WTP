using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.DAL.Services;
using WTP.WebApi.WTP.DAL.DomainModels;

namespace WTP.WebApi.WTP.DAL.Services.TeamService
{
    public class TeamService : IMaintainable<Team>
    {
        public async Task<bool> CreateAsync(Team obj)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Team>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Team> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(Team obj)
        {
            throw new NotImplementedException();
        }
    }
}
