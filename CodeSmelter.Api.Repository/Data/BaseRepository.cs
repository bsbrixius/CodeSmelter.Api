using CodeSmelter.Api.Repository.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CodeSmelter.Api.Repository.Data
{
    public class BaseRepository<TEntity, TDbContext> : IRepository<TEntity>
            where TEntity : Entity, IAggregateRoot
            where TDbContext : BaseDb
    {
        protected DbSet<TEntity> _dbSet { get; }

        private readonly TDbContext _context;

        public BaseRepository(TDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> Query
        {
            get
            {
                return _dbSet;
            }
        }

        public virtual IQueryable<TEntity> QueryNoTrack
        {
            get
            {
                return _dbSet.AsNoTracking();
            }
        }

        public IUnitOfWork UnitOfWork => _context;

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            //if (entity.Id == default)
            //{
            //    entity.Id = Guid.NewGuid();
            //}

            await _dbSet.AddAsync(entity);
            return entity;
        }

        public virtual async Task<List<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<List<TEntity>> GetAllIncludeAsync(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            if (includeProperties.Any())
            {
                return await Include(_context.Set<TEntity>(), includeProperties)
                    .ToListAsync();
            }

            return await _context.Set<TEntity>()
                .ToListAsync();
        }

        public virtual async Task<List<TEntity>> GetAllByAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (includeProperties.Any())
            {
                query = Include(query, includeProperties);
            }

            if (where != null)
            {
                query = query.Where(where);
            }

            return await query
                .ToListAsync();
        }

        private IQueryable<TEntity> Include(IQueryable<TEntity> query, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            foreach (Expression<Func<TEntity, object>> property in includeProperties)
            {
                query = query.Include(property);
            }

            return query;
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public virtual async Task<bool> ExistsAsync(int id)
        {
            return await Task.Run(() => _dbSet.Any(x => x.Id == id));
        }

        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
        }

        public bool Commit()
        {
            return _context.SaveChanges() > 0;
        }
        public async Task<bool> CommitAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }
    }
}
