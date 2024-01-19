using LinkShorteningSystem.Domain.Entities;
using LinkShorteningSystem.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace LinkShorteningSystem.Infrastructure.Data.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        private readonly CatalogDbContext _dbContext;
        private readonly DbSet<T> _table;

        public BaseRepository(
            CatalogDbContext dbContext)
        {
            _dbContext = dbContext;
            _table = _dbContext.Set<T>();
        }

        public async Task<T> GetOneByAsync(Func<IQueryable<T>,
            IIncludableQueryable<T, object>> include = null,
            Expression<Func<T, bool>> expression = null,
            CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = _table;

            if (expression is not null)
            {
                query = query.Where(expression);
            }

            if (include is not null)
            {
                query = include(query);
            }

            var model = await query.AsNoTracking()
                                   .FirstOrDefaultAsync(cancellationToken);

            return model;
        }

        public async Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _table.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            _dbContext.Attach(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            _table.Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}