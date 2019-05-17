﻿using AutoMapper;
using WTP.BLL.Models.AppUserModels;
using WTP.BLL.UnitOfWork;

namespace WTP.DAL.Repositories.ConcreteRepositories.AppUserExtended
{
    internal class AppUserRepository : RepositoryBase<AppUserModel>, IRepository<AppUserModel>
    {
        public AppUserRepository(ApplicationDbContext context, IMapper mapper)
            : base(context, mapper)
        {
        }
    }
}
