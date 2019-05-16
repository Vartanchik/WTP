using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WTP.DAL.UnitOfWork;

namespace WTP.BLL.Services.DeletService
{
    public class DeletService : IDeleteService
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
