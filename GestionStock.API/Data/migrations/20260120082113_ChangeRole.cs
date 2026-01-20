using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionStock.API.data.migrations
{
    /// <inheritdoc />
    public partial class ChangeRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Username",
                keyValue: "restocker",
                column: "Roles",
                value: 4);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Username",
                keyValue: "restocker",
                column: "Roles",
                value: 1);
        }
    }
}
