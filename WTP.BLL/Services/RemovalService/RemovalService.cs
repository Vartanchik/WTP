using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.BLL.UnitOfWork;

namespace WTP.BLL.Services.RemovalService
{
    public class DeletService : IRemovalService
    {
        private readonly DateTime _storageTime;
        private readonly IUnitOfWork _ouw;

        public DeletService(DateTime storageTime, IUnitOfWork unitOfWork)
        {
            _storageTime = storageTime;
            _ouw = unitOfWork;
        }

        public Task<List<int>> GetIdToDeletAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<int>> GetIdToDeletTimeLeftAsync(DateTime dateToDelet)
        {
            throw new NotImplementedException();
        }
    }
}
