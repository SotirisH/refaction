using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Xero.AspNet.Core.Data
{
    /// <summary>
    /// Implementation of the Async methods
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="DB"></typeparam>
    public partial class GenericRepository<T, DB> : IRepository<T> where T : EntityBase
                                                     where DB : DbContext, IAuditableDBContext, new()
    {
        public async Task<T> GetAsync(Expression<Func<T, bool>> where)
        {
            return await dbSet.SingleOrDefaultAsync(where);
        }

        public async Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> where)
        {
            return await dbSet.Where(where).ToArrayAsync();
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbSet.ToArrayAsync();
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> where)
        {
            return await dbSet.AnyAsync(where);
        }


    }
}
