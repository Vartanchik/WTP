using WTP.DAL.Entities;
using WTP.DAL.Repositories.GenericRepository;

namespace WTP.DAL.Repositories.ConcreteRepositories
{
    internal class ServerRepository : RepositoryBase<Server>, IRepository<Server>
    {
        public ServerRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}
