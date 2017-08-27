﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Xero.AspNet.Core.Data
{
    /// <summary>
    /// Factory responsible for managing the instance of DbContext.
    /// Supports Init and dispose funtionality and also can generate Generic repositories
    /// </summary>
    public abstract class DbFactory<DB> : Disposable where DB : DbContext, IAuditableDBContext, new()
    {
        internal DB dbContext;

        /// <summary>
        /// Returns the active DBContext Or Initializes a new instance
        /// </summary>
        /// <returns></returns>
        /// <remarks> The DBContext life time should be request, so in the very first time the object is created and
        /// then the same instance is used during the request(Kind of signleton pattern)
        /// </remarks>
        public virtual DB DBContext
        {
            get
            {
                if (dbContext == null)
                {
                    dbContext = Init();
                }
                return dbContext;
            }
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }

        protected virtual DB Init()
        {
            return new DB();
        }

        public Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        /// <summary>
        /// Returns a generic repository from the dictionary and if does not exits then it creates one
        /// </summary>
        public GenericRepository<TEntity, DB> GetGenericRepositoryOf<TEntity>() where TEntity : EntityBase
        {
            if (repositories.Keys.Contains(typeof(TEntity)) == true)
            {
                return repositories[typeof(TEntity)] as GenericRepository<TEntity, DB>;
            }
            IRepository<TEntity> r = new GenericRepository<TEntity, DB>(this);
            repositories.Add(typeof(TEntity), r);
            return r as GenericRepository<TEntity, DB>;
        }
    }
}