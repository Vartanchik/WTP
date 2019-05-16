using WTP.DAL.Entities.AppUserEntities;
using WTP.DAL.Repositories.GenericRepository;

namespace WTP.DAL.Repositories.ConcreteRepositories.AppUserExtended
{
    internal class DeletUserRepository : RepositoryBase<DeletedUser>, IRepository<DeletedUser>
    {
        public DeletUserRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}
