using WTP.DAL.Entities.AppUserEntities;
using WTP.DAL.Repositories.GenericRepository;

namespace WTP.DAL.Repositories.ConcreteRepositories
{
    internal class CountryRepository : RepositoryBase<Country>, IRepository<Country>
    {
        public CountryRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}
