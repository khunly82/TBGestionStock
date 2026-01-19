using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GestionStock.API.data.migrations
{
    /// <inheritdoc />
    public partial class UserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EncodedPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Roles = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Username);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Username", "EncodedPassword", "Roles" },
                values: new object[,]
                {
                    { "admin", "5a2911ea-6a3e-4dc4-a9c2-e7dc51dcb5b6fsnT6pHI3P3XjmRNPE0XuifF0X9rLBNbLnWUYOMPFwWop8jPb0qRAtF9yHlIIkPEHbt07ihzmHcnkFf2Rc7Kwg==", 1 },
                    { "restocker", "4f6e1151-c9a8-4e8d-8ab9-7772a042b3c0UbMK9bX3e/aTi+F+wbVcYIg5JjsaL8M9O+fkMEoTqtcUJoD/LjE3TcQ9eKqSxWSRTkTNuNm1778KnrRkONW8eQ==", 1 },
                    { "seller", "04a0506f-901c-4879-ad7b-89fedbe4672eNX4lmEE3w3gip6ZmHZfdNedqqmA2HujSaMlQ8di3qiQSr0RZfB9WcWVhGKBMaNfQ3PXRejQ7Rsw+XKtSmQKOUw==", 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
