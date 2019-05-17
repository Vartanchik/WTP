using AutoMapper;
using WTP.BLL.Models.TeamModels;
using WTP.BLL.UnitOfWork;

namespace WTP.DAL.Repositories.ConcreteRepositories.TeamExtended
{
    internal class TeamRepository : RepositoryBase<TeamModel>, IRepository<TeamModel>
    {
        public TeamRepository(ApplicationDbContext context, IMapper mapper)
            : base(context, mapper)
        { }
    }
}
