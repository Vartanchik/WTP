using System;
using System.Linq;
using System.Threading.Tasks;

namespace WTP.BLL.Services.DeleteService
{
    interface IDeleteService
    {
        IQueryable<int> FindToDeletedAsync();
        IQueryable<int> FindToDeletedByIntervalAsync(TimeSpan intervalToDelete);
        Task DeleteOverdue();
        Task DeleteOverdue(int id);
    }
}
