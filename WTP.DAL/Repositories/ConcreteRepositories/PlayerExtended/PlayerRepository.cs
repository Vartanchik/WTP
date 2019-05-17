using AutoMapper;
using WTP.BLL.Models.PlayerModels;
using WTP.BLL.UnitOfWork;

namespace WTP.DAL.Repositories.ConcreteRepositories
{
    internal class PlayerRepository : RepositoryBase<PlayerModel>, IRepository<PlayerModel>
    {
        public PlayerRepository(ApplicationDbContext context, IMapper mapper)
            : base(context, mapper)
        { }
    }
}
