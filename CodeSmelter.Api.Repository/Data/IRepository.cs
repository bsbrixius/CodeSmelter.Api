using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CodeSmelter.Api.Repository.Data
{
    public interface IRepository<TEntity> : IDisposable where TEntity : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
        IQueryable<TEntity> Query { get; }
        IQueryable<TEntity> QueryNoTrack { get; }
        bool Commit();
        Task<bool> CommitAsync();
        Task<TEntity> AddAsync(TEntity entity);
        Task<List<TEntity>> GetAllAsync();
        Task<List<TEntity>> GetAllIncludeAsync(params Expression<Func<TEntity, object>>[] includeProperties);
        Task<List<TEntity>> GetAllByAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties);
        //Task DeleteAsync(TEntity entity);
        Task<TEntity> GetByIdAsync(int id);
        Task<bool> ExistsAsync(int id);
        void Update(TEntity entity);
    }
}