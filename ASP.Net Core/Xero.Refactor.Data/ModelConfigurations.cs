/* Data configuration of all 
 * EF models using Fluent API.
 * Why?: Everything what you can configure with DataAnnotations is also possible with the Fluent API. 
 * The reverse is not true. So, from the viewpoint of configuration options and flexibility the Fluent API is "better".
 * Also the validation rules are out of the POCO class
 * Help:https://msdn.microsoft.com/en-us/data/jj591617.aspx#PropertyIndex
 */

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xero.Refactor.Data.Models;

namespace Xero.Refactor.Data
{

    /// <summary>
    /// Fluent API for setting up Product
    /// </summary>
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            {
                builder.ToTable("Product");
                builder.HasKey(p => p.Id);
                //Property(p => p.Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
                builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
                builder.Property(p => p.Description).HasMaxLength(500);
            }
        }
    }
    /// <summary>
    /// Fluent API for setting up ProductOption
    /// </summary>
    public class ProductOptionConfiguration : IEntityTypeConfiguration<ProductOption>
    {
        public void Configure(EntityTypeBuilder<ProductOption> builder)
        {
            builder.ToTable("ProductOption");
            builder.HasKey(p => p.Id);
            //Property(p => p.Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Description).HasMaxLength(500);
            // Create ForeignKey using fluent API on Property 
            // http://www.entityframeworktutorial.net/code-first/configure-one-to-many-relationship-in-code-first.aspx 
            builder.Property(p => p.ProductId).IsRequired();
            //https://docs.microsoft.com/en-us/ef/core/modeling/relationships
            builder.HasOne(p => p.Product)
                .WithMany(s => s.ProductOptions)
                .HasForeignKey(s => s.ProductId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
