using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WTP.BLL.Services.RemovalService
{
    interface IRemovalService
    {
        Task<List<int>> GetIdToDeletAsync();
        Task<List<int>> GetIdToDeletTimeLeftAsync(DateTime dateToDelet);
    }
}
