﻿using System;
using System.Linq;
using System.Threading.Tasks;
using WTP.DAL.Entities.AppUserEntities;
using WTP.DAL.Repositories.GenericRepository;

namespace WTP.DAL.Repositories.ConcreteRepositories.RefreshTokenRepository
{
    public interface IRefreshTokenRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        Task DeleteUserTokensAsync(int userId);
        IQueryable<TEntity> GetUserTokensAsync(int id);
        Task<RefreshToken> UserTokenByValue(int userId, string refreshToken);
        int GetIdByCondition(Func<TEntity, bool> condition);
    }
}
