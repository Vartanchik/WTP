using AutoMapper;
using WTP.BLL.Models.AppUserModels;
using WTP.BLL.UnitOfWork;

namespace WTP.DAL.Repositories.ConcreteRepositories
{
    internal class LanguageRepository : RepositoryBase<LanguageModel>, IRepository<LanguageModel>
    {
        public LanguageRepository(ApplicationDbContext context, IMapper mapper)
            : base(context, mapper)
        {
        }
    }
}
