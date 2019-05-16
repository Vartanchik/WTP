using WTP.DAL.Entities.TeamEntities;
using WTP.DAL.Repositories.GenericRepository;

namespace WTP.DAL.Repositories.ConcreteRepositories.TeamExtended
{
    internal class TeamRepository : RepositoryBase<Team>, IRepository<Team>
    {
        public TeamRepository(ApplicationDbContext context) : base(context)
        { }
    }
}
