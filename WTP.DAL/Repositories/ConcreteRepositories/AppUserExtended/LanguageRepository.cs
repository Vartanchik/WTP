using WTP.DAL.Entities.AppUserEntities;
using WTP.DAL.Repositories.GenericRepository;

namespace WTP.DAL.Repositories.ConcreteRepositories
{
    internal class LanguageRepository : RepositoryBase<Language>, IRepository<Language>
    {
        public LanguageRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}
