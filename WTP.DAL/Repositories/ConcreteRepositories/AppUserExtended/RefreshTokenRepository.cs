using AutoMapper;
using WTP.BLL.Models.AppUserModels;
using WTP.BLL.UnitOfWork;

namespace WTP.DAL.Repositories.ConcreteRepositories.AppUserExtended
{
    internal class RefreshTokenRepository : RepositoryBase<RefreshTokenModel>, IRepository<RefreshTokenModel>
    {
        public RefreshTokenRepository(ApplicationDbContext context, IMapper mapper)
            : base(context, mapper)
        {
        }
    }
}
