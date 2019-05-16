using WTP.DAL.Entities;
using WTP.DAL.Repositories.GenericRepository;

namespace WTP.DAL.Repositories.ConcreteRepositories
{
    internal class GameRepository : RepositoryBase<Game>, IRepository<Game>
    {
        public GameRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}
