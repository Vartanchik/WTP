using WTP.DAL.DomainModels;
using WTP.DAL.Repositories.GenericRepository;

namespace WTP.DAL.Repositories.ConcreteRepositories
{
    internal class PlayerRepository : RepositoryBase<Player>, IRepository<Player>
    {
        public PlayerRepository(ApplicationDbContext context) : base(context)
        { }
    }
}
