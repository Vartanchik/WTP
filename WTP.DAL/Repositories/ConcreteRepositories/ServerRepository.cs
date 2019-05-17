using AutoMapper;
using WTP.BLL.Models;
using WTP.BLL.UnitOfWork;

namespace WTP.DAL.Repositories.ConcreteRepositories
{
    internal class ServerRepository : RepositoryBase<ServerModel>, IRepository<ServerModel>
    {
        public ServerRepository(ApplicationDbContext context, IMapper mapper)
            : base(context, mapper)
        {
        }
    }
}
