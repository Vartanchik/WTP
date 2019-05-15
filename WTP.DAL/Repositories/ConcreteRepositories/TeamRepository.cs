using WTP.DAL.Entities;
using WTP.DAL.Repositories.GenericRepository;

namespace WTP.DAL.Repositories.ConcreteRepositories
{
    internal class TeamRepository : RepositoryBase<Team>, IRepository<Team>
    {
        public TeamRepository(ApplicationDbContext context) : base(context)
        { }
    }
}
