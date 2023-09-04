using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LinkShorteningSystem.Domain.Interfaces.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T> AddAsync(T entity);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetOneByAsync(Func<IQueryable<T>,
            IIncludableQueryable<T, object>>? include = null,
            Expression<Func<T, bool>>? expression = null);
    }
}