using AutoMapper;
using WTP.BLL.Models;
using WTP.BLL.UnitOfWork;

namespace WTP.DAL.Repositories.ConcreteRepositories
{
    internal class GoalRepository : RepositoryBase<GoalModel>, IRepository<GoalModel>
    {
        public GoalRepository(ApplicationDbContext context, IMapper mapper)
            : base(context, mapper)
        {
        }
    }
}
