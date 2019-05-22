using System;
using System.Linq;
using System.Threading.Tasks;

namespace WTP.BLL.Services.DeleteService
{
    public interface IDeleteService
    {
        IQueryable<int> FindUsersToDeletedAsync();
        IQueryable<int> FindUsersToDeletedByIntervalAsync(TimeSpan intervalToDelete);
        Task DeleteOverdueUsers();
        Task DeleteOverdueUser(int userId);
    }
}
