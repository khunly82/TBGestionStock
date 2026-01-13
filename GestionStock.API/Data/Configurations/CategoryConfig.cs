using GestionStock.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionStock.API.Data.Configurations
{
    public class CategoryConfig : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id).IsClustered(false);
            builder.Property(c => c.Name)
                .IsUnicode(false)
                .HasMaxLength(255);

            builder.HasIndex(c => c.Name)
                // par defaut les entités sont triés sur l'id
                // les trier sur le nom par défaut
                .IsClustered(true)
                .IsUnique();

            // valeurs par defaut dans la db
            builder.HasData([
                new () { Id = 1, Name = "Biere" },    
                new () { Id = 2, Name = "Vin" },
                new () { Id = 3, Name = "Alcool" },
                new () { Id = 4, Name = "Soft" },
            ]);
        }
    }
}
