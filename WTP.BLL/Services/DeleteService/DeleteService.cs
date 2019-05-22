using System;
using System.Linq;
using System.Threading.Tasks;
using WTP.DAL.UnitOfWork;

namespace WTP.BLL.Services.DeleteService
{
    public class DeleteService : IDeleteService
    {
        private readonly TimeSpan _interval;
        private readonly IUnitOfWork _uof;

        public DeleteService(TimeSpan interval, IUnitOfWork unitOfWork)
        {
            _interval = interval;
            _uof = unitOfWork;
        }

        public async Task DeleteOverdueUsers()
        {
            var usersIdToDelete = _uof.AppUsers.AsQueryable()
                .Where(x => x.IsDeleted == true 
                    && x.DeletedTime.Value.Add(_interval) >= DateTime.Today)
                .Select(x => x.Id);

            foreach (var item in usersIdToDelete)
            {
                await _uof.AppUsers.DeleteAsync(item);
            }
        }

        public async Task DeleteOverdueUser(int userId)
        {
            var user = await _uof.AppUsers.GetByIdAsync(userId);

            if (user.IsDeleted == true && user.DeletedTime.Value.Add(_interval) > DateTime.Today)
            {
                await _uof.AppUsers.DeleteAsync(user.Id);
            }
        }

        public IQueryable<int> FindUsersToDeletedAsync()
        {
            return _uof.AppUsers.AsQueryable()
                .Where(x => x.IsDeleted == true)
                .Select(x => x.Id);
        }

        public IQueryable<int> FindUsersToDeletedByIntervalAsync(TimeSpan intervalToDelete)
        {
            return _uof.AppUsers.AsQueryable()
                .Where(x => x.IsDeleted == true 
                    && x.DeletedTime.Value.Add(_interval) == DateTime.Today.Add(intervalToDelete))
                .Select(x => x.Id);
        }
    }
}
