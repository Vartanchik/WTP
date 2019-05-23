﻿using System;
using System.Threading.Tasks;
using WTP.DAL.Entities;
using WTP.DAL.Repositories.ConcreteRepositories;
using WTP.DAL.Repositories.ConcreteRepositories.PlayerRepository;
using WTP.DAL.Repositories.GenericRepository;

namespace WTP.DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository<AppUser> AppUsers { get; }
        IRepository<Country> Countries { get; }
        IRepository<Gender> Genders { get; }
        IRepository<Language> Languages { get; }
        IPlayerRepository<Player> Players { get; }
        IRepository<Team> Teams { get; }
        ITokenRepository<RefreshToken> Tokens { get; }
        IRepository<Comment> Comments { get; }
        IRepository<Match> Matches { get; }
        IRepository<Game> Games { get; }
        IRepository<Server> Servers { get; }
        IRepository<Goal> Goals { get; }
        IRepository<Rank> Ranks { get; }

        void Commit();

        Task CommitAsync();
    }
}
