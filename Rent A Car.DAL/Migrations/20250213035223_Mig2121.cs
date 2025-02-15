using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rent_A_Car.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Mig2121 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "WishLists");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "WishLists");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "WishLists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "WishLists",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
