using System;
using System.Collections.Generic;
using System.Text;
using WTP.DAL.Entities.AppUserEntities;
using WTP.DAL.Repositories.GenericRepository;

namespace WTP.DAL.Repositories.ConcreteRepositories.AppUserExtended
{
    internal interface IAppUserRepository : IRepository<AppUser>
    {
        // some additional functionality for the AppUserRepository
    }
}
