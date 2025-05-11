using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FERSOFT.ERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NuevoBasedeDatos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_ClientEntity_ClientId",
                table: "Bookings");

            migrationBuilder.DropTable(
                name: "ClientEntity");

            migrationBuilder.AddColumn<int>(
                name: "Int",
                table: "Movies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Customers_ClientId",
                table: "Bookings",
                column: "ClientId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Customers_ClientId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Int",
                table: "Movies");

            migrationBuilder.CreateTable(
                name: "ClientEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientEntity", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_ClientEntity_ClientId",
                table: "Bookings",
                column: "ClientId",
                principalTable: "ClientEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
