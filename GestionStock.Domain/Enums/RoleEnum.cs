namespace GestionStock.Domain.Enums
{
    [Flags]
    public enum RoleEnum
    {
        Admin = 1,
        Seller = 2,
        Restocker = 4,
    }
}
