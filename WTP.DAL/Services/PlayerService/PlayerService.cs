using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.DAL.Services;
using WTP.WebApi.WTP.DAL.DomainModels;

namespace WTP.WebApi.WTP.DAL.Services.PlayerService
{
    public class PlayerService : IMaintainable<Player>
    {
        public async Task<bool> CreateAsync(Player obj)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Player>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Player> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(Player obj)
        {
            throw new NotImplementedException();
        }
    }
}
