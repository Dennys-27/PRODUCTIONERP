using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FERSOFT.ERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Actualizarmodelos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BillboardEntityId",
                table: "Seats",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Seats",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOccupied",
                table: "Seats",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "BillboardEntityId",
                table: "Bookings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MovieId",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FunctionDate",
                table: "Billboards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MovieTitle",
                table: "Billboards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ClientEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientEntity", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Seats_BillboardEntityId",
                table: "Seats",
                column: "BillboardEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_BillboardEntityId",
                table: "Bookings",
                column: "BillboardEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ClientId",
                table: "Bookings",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_MovieId",
                table: "Bookings",
                column: "MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Billboards_BillboardEntityId",
                table: "Bookings",
                column: "BillboardEntityId",
                principalTable: "Billboards",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_ClientEntity_ClientId",
                table: "Bookings",
                column: "ClientId",
                principalTable: "ClientEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Movies_MovieId",
                table: "Bookings",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_Billboards_BillboardEntityId",
                table: "Seats",
                column: "BillboardEntityId",
                principalTable: "Billboards",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Billboards_BillboardEntityId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_ClientEntity_ClientId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Movies_MovieId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Seats_Billboards_BillboardEntityId",
                table: "Seats");

            migrationBuilder.DropTable(
                name: "ClientEntity");

            migrationBuilder.DropIndex(
                name: "IX_Seats_BillboardEntityId",
                table: "Seats");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_BillboardEntityId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_ClientId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_MovieId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "BillboardEntityId",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "IsOccupied",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "BillboardEntityId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "MovieId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "FunctionDate",
                table: "Billboards");

            migrationBuilder.DropColumn(
                name: "MovieTitle",
                table: "Billboards");
        }
    }
}
