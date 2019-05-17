using AutoMapper;
using WTP.BLL.Models.AppUserModels;
using WTP.BLL.UnitOfWork;

namespace WTP.DAL.Repositories.ConcreteRepositories
{
    internal class CountryRepository : RepositoryBase<CountryModel>, IRepository<CountryModel>
    {
        public CountryRepository(ApplicationDbContext context, IMapper mapper)
            : base(context, mapper)
        {
        }
    }
}
