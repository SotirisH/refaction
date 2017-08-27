using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Xero.AspNet.Core.Data
{
    /// <summary>
    /// Extended DbContext that adds timestamps on the EntityBase classes when they are saved
    /// </summary>
    public abstract class AuditableDbContext : DbContext, ISupportsUnitOfWork
    {
        private readonly ICurrentUserService _currentUserService;

        protected AuditableDbContext() : base()
        { }

        protected AuditableDbContext(DbContextOptions options,
                                    ICurrentUserService currentUserService) : base(options)
        {
            _currentUserService = currentUserService ?? throw new ArgumentNullException("currentUserService");
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Validates all models, adds tracking record and saves the changes
        /// This is called last when we save the changes
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess"></param>
        /// <returns></returns>

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ValidateModel();
            AddAudit();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        /// <summary>
        /// All entities are being audited.
        /// This overload should be used instead of the original one
        /// </summary>
        /// <param name="userName">The name of the user that perform the actions</param>
        /// <returns></returns>
        private void AddAudit()
        {
            var currentUser = _currentUserService.GetCurrentUser();
            var changeSet = ChangeTracker.Entries<EntityBase>();


            if (changeSet != null)
            {
                foreach (var entry in changeSet.Where(c => c.State == EntityState.Added))
                {
                    entry.Entity.CreatedOn = DateTime.Now;
                    entry.Entity.CreatedBy = currentUser;
                }
                foreach (var entry in changeSet.Where(c => c.State == EntityState.Modified))
                {
                    entry.Entity.ModifiedOn = DateTime.Now;
                    entry.Entity.ModifiedBy = currentUser;
                }
            }
        }

        /// <summary>
        /// Validated the model. 
        /// An ValidationException is thrown if the validation doesn't pass
        /// </summary>
        /// <remarks>
        /// https://blogs.msmvps.com/ricardoperes/2016/04/25/implementing-missing-features-in-entity-framework-core-part-3/</remarks>
        /// 
        public void ValidateModel()
        {
            var serviceProvider = this.GetService<IServiceProvider>();
            var items = new Dictionary<object, object>();
            var errorResults = new List<ValidationResult>();
            foreach (var entry in ChangeTracker.Entries().Where(e => (e.State == EntityState.Added) || (e.State == EntityState.Modified)))
            {
                var entity = entry.Entity;
                var context = new ValidationContext(entity, serviceProvider, items);
                var results = new List<ValidationResult>();

                if (Validator.TryValidateObject(entity, context, results, true) == false)
                {
                    foreach (var result in results)
                    {
                        if (result != ValidationResult.Success)
                        {
                            throw new ValidationException(result, null, result.ErrorMessage);
                        }
                    }
                }
            }
        }


    }
}
