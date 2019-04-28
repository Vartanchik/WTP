﻿using System;
using System.Threading.Tasks;
using WTP.DAL.DomainModels;
using WTP.DAL.Repositories.GenericRepository;

namespace WTP.DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<AppUser> AppUsers { get; }
        IRepository<Country> Countries { get; }
        IRepository<Gender> Genders { get; }
        IRepository<Language> Languages { get; }
        IRepository<Player> Players { get; }
        IRepository<Team> Teams { get; }
        IRepository<Admin> Admins { get; }
        IRepository<AdminOperation> AdminOperations { get; }
        IRepository<History> Histories { get; }

        void Commit();

        Task CommitAsync();
    }
}
