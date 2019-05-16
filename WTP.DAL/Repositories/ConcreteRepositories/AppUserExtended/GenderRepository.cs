using WTP.DAL.Entities.AppUserEntities;
using WTP.DAL.Repositories.GenericRepository;

namespace WTP.DAL.Repositories.ConcreteRepositories
{
    internal class GenderRepository : RepositoryBase<Gender>, IRepository<Gender>
    {
        public GenderRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}
