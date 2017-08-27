using Microsoft.EntityFrameworkCore;
using Xero.AspNet.Core;
using Xero.AspNet.Core.Data;
using Xero.Refactor.Data.Models;

namespace Xero.Refactor.Data
{
    public class RefactorDb : AuditableDbContext
    {
        /// <summary>
        /// Internal costructor for Migration commands
        /// </summary>
        internal RefactorDb() : base()
        {

        }
        public RefactorDb(DbContextOptions<RefactorDb> options,
                              ICurrentUserService currentUserService) : base(options, currentUserService)
        {
            //https://stackoverflow.com/questions/41513296/can-i-safely-use-the-non-generic-dbcontextoptions-in-asp-net-core-and-ef-core
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var catalog = @"I:\GITREPO\XERO\REFACTION\ASP.NET CORE\XERO.REFACTOR.WEBAPI.CORE\DATA\DATABASE.MDF";
                // When we run the migration commands these are executed in the dev DB
                optionsBuilder.UseSqlServer($@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog={catalog};Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }

     

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductOption> ProductOptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new ProductOptionConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}