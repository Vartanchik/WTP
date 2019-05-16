using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WTP.BLL.Services.DeletService
{
    interface IDeleteService
    {
        Task<List<int>> GetIdToDeletAsync();
        Task<List<int>> GetIdToDeletTimeLeftAsync(DateTime dateToDelet);
    }
}
