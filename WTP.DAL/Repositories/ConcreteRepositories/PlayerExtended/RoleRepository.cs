using AutoMapper;
using WTP.BLL.Models.PlayerModels;
using WTP.BLL.UnitOfWork;

namespace WTP.DAL.Repositories.PlayerExtended
{
    internal class RoleRepository : RepositoryBase<PlayerRoleModel>, IRepository<PlayerRoleModel>
    {
        public RoleRepository(ApplicationDbContext context, IMapper mapper)
            : base(context, mapper)
        {
        }
    }
}
