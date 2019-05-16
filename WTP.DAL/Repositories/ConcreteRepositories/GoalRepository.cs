using WTP.DAL.Entities;
using WTP.DAL.Repositories.GenericRepository;

namespace WTP.DAL.Repositories.ConcreteRepositories
{
    internal class GoalRepository : RepositoryBase<Goal>, IRepository<Goal>
    {
        public GoalRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}
