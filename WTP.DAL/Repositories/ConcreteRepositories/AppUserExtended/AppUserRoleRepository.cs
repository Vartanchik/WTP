using AutoMapper;
using WTP.BLL.Models.AppUserModels;
using WTP.BLL.UnitOfWork;

namespace WTP.DAL.Repositories.ConcreteRepositories.AppUserExtended
{
    class AppUserRoleRepository : RepositoryBase<AppUserRoleModel>, IRepository<AppUserRoleModel>
    {
        public AppUserRoleRepository(ApplicationDbContext context, IMapper mapper)
            : base(context, mapper)
        {
        }
    }
}
