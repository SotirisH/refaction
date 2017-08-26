using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Xero.AspNet.Core.Data
{
    public interface IRepository<T> where T : EntityBase
    {
        // Marks an entity as new
        void Add(T entity);

        // Marks an entity as modified
        void Update(T entity);

        // Marks an entity to be removed
        void Delete(T entity);

        void Delete(Expression<Func<T, bool>> where);

        // Get an entity by int id
        T GetById(object id, bool throwExceptionIfNotFound = false);

        // Get an entity using delegate
        T Get(Expression<Func<T, bool>> where);

        T GetByEntity(T entity);

        // Gets all entities of type T
        IEnumerable<T> GetAll();

        // Gets entities using delegate
        IEnumerable<T> GetMany(Expression<Func<T, bool>> where);

        IQueryable<T> GetAsQueryable();

        /// <summary>
        /// Gets a single entity in an asychronous way.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        Task<T> GetAsync(Expression<Func<T, bool>> where);
        /// <summary>
        /// Gets multiple entities in an asychronous way
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> where);
        /// <summary>
        /// Gets all the entities in an asynchronous way
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Checks if the entity exists
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(Expression<Func<T, bool>> where);
    }
}