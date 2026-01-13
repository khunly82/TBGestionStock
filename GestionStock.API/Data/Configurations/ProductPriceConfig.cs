using GestionStock.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionStock.API.Data.Configurations
{
    public class ProductPriceConfig : IEntityTypeConfiguration<ProductPrice>
    {
        public void Configure(EntityTypeBuilder<ProductPrice> builder)
        {
            builder.Property(p => p.Price)
                .HasColumnType("DECIMAL(7,2)"); // xxxxx,xx
        }
    }
}
