using System.Data.Common;
using System.Data.Entity;
using Xero.AspNet.Core.Data;
using Xero.Refactor.Data.Models;

namespace Xero.Refactor.Data
{
    public class RefactorDb : AuditableDbContext
    {
        public RefactorDb() : base("RefactorDb")
        {
        }

        /// <summary>
        /// Constructor for creating memory dB
        /// </summary>
        /// <param name="connection"></param>
        public RefactorDb(DbConnection connection) : base(connection, true)
        {
        }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductOption> ProductOptions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new ProductConfiguration());
            modelBuilder.Configurations.Add(new ProductOptionConfiguration());
        }
    }
}