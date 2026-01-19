using GestionStock.Domain.Enums;

namespace GestionStock.Domain.Entities
{
    public class User
    {
        public required string Username { get; set; }
        public required string EncodedPassword { get; set; }
        public required RoleEnum Roles { get; set; } = RoleEnum.Seller | RoleEnum.Restocker;
    }
}
