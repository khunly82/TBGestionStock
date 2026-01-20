using GestionStock.Domain.Entities;
using GestionStock.Domain.Enums;
using GestionStock.Utils.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionStock.API.Data.Configurations
{
    internal class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Username);


            builder.HasData([
                new User { Username = "admin", Roles = RoleEnum.Admin, EncodedPassword = PasswordUtils.HashPassword("1234", Guid.Parse("5a2911ea-6a3e-4dc4-a9c2-e7dc51dcb5b6")) },
                new User { Username = "seller", Roles = RoleEnum.Seller, EncodedPassword = PasswordUtils.HashPassword("1234", Guid.Parse("04a0506f-901c-4879-ad7b-89fedbe4672e")) },
                new User { Username = "restocker", Roles = RoleEnum.Restocker, EncodedPassword = PasswordUtils.HashPassword("1234", Guid.Parse("4f6e1151-c9a8-4e8d-8ab9-7772a042b3c0")) },
            ]);
        }
    }
}
