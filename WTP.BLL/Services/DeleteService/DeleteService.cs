using System;
using System.Linq;
using System.Threading.Tasks;
using WTP.DAL.UnitOfWork;

namespace WTP.BLL.Services.DeleteService
{
    public class DeleteService : IDeleteService
    {
        private readonly TimeSpan _interval;
        private readonly IUnitOfWork _uow;

        public DeleteService(TimeSpan interval, IUnitOfWork unitOfWork)
        {
            _interval = interval;
            _uow = unitOfWork;
        }

        public async Task DeleteOverdueUsers()
        {
            var usersIdToDelete = _uow.AppUsers.AsQueryable()
                .Where(x => x.IsDeleted == true 
                    && x.DeletedTime.Value.Add(_interval) >= DateTime.Today)
                .Select(x => x.Id);

            foreach (var item in usersIdToDelete)
            {
                await _uow.AppUsers.DeleteAsync(item);
            }
        }

        public IQueryable<int> FindUsersToDeletedAsync()
        {
            return _uow.AppUsers.AsQueryable()
                .Where(x => x.IsDeleted == true)
                .Select(x => x.Id);
        }

        public IQueryable<int> FindUsersToDeletedByIntervalAsync(TimeSpan intervalToDelete)
        {
            return _uow.AppUsers.AsQueryable()
                .Where(x => x.IsDeleted == true 
                    && x.DeletedTime.Value.Add(_interval) == DateTime.Today.Add(intervalToDelete))
                .Select(x => x.Id);
        }
    }
}
