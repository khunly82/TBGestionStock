using GestionStock.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionStock.API.Data.Configurations
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Name)
                .HasMaxLength(255)
                .IsUnicode(false); // VARCHAR(255)

            builder.Property(p => p.Reference)
                .HasMaxLength(8)
                .IsFixedLength(true)
                .IsUnicode(false); // CHAR(8)

            builder.HasQueryFilter(p => !p.IsDeleted);
        }
    }
}
