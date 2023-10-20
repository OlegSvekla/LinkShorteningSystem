using LinkShorteningSystem.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LinkShorteningSystem.Infrastructure.Data
{
    public sealed class EfRepository<T> : IRepository<T> where T : class
    {
        private readonly CatalogContext _context;
        private readonly DbSet<T> _dbSet;

        public EfRepository(CatalogContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T> GetOneByAsync(Func<IQueryable<T>,
            IIncludableQueryable<T, object>>? include = null,
            Expression<Func<T, bool>>? expression = null)
        {
            IQueryable<T> query = _dbSet;

            if (expression is not null)
            {
                query = query.Where(expression);
            }

            if (include is not null)
            {
                query = include(query);
            }

            var model = await query.AsNoTracking().FirstOrDefaultAsync();

            return model!;
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }
    }
}