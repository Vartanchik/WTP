using AutoMapper;
using WTP.BLL.Models;
using WTP.BLL.UnitOfWork;

namespace WTP.DAL.Repositories.ConcreteRepositories
{
    internal class GameRepository : RepositoryBase<GameModel>, IRepository<GameModel>
    {
        public GameRepository(ApplicationDbContext context, IMapper mapper)
            : base(context, mapper)
        {
        }
    }
}
