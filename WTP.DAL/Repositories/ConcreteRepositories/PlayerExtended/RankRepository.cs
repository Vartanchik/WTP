using AutoMapper;
using WTP.BLL.Models.PlayerModels;
using WTP.BLL.UnitOfWork;

namespace WTP.DAL.Repositories.PlayerExtended
{
    internal class RankRepository : RepositoryBase<RankModel>, IRepository<RankModel>
    {
        public RankRepository(ApplicationDbContext context, IMapper mapper)
            : base(context, mapper)
        {
        }
    }
}
