using System.Data.Entity.ModelConfiguration;
using Xero.Refactor.Data.Models;

namespace Xero.Refactor.Data
{

    /// <summary>
    /// Fluent API for setting up Product
    /// </summary>
    public class ProductConfiguration : EntityTypeConfiguration<Product>
    {
        public ProductConfiguration()
        {
            HasKey(p => p.Id);
            Property(p => p.Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(p => p.Name).IsRequired().HasMaxLength(100);
            Property(p => p.Description).HasMaxLength(500);
        }
    }

    /// <summary>
    /// Fluent API for setting up ProductOption
    /// </summary>
    public class ProductOptionConfiguration : EntityTypeConfiguration<ProductOption>
    {
        public ProductOptionConfiguration()
        {
            HasKey(p => p.Id);
            Property(p => p.Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(p => p.Name).IsRequired().HasMaxLength(100);
            Property(p => p.Description).HasMaxLength(500);
        }
    }
}
