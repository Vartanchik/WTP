using AutoMapper;
using WTP.BLL.Models.PlayerModels;
using WTP.BLL.UnitOfWork;

namespace WTP.DAL.Repositories.PlayerExtended
{
    internal class CommentRepository : RepositoryBase<CommentModel>, IRepository<CommentModel>
    {
        public CommentRepository(ApplicationDbContext context, IMapper mapper)
            : base(context, mapper)
        {
        }
    }
}
