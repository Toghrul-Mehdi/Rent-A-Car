using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rent_A_Car.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Elanlar_Sekiller1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advertisements_AspNetUsers_UserId1",
                table: "Advertisements");

            migrationBuilder.DropIndex(
                name: "IX_Advertisements_UserId1",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Advertisements");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Advertisements",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_UserId",
                table: "Advertisements",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisements_AspNetUsers_UserId",
                table: "Advertisements",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advertisements_AspNetUsers_UserId",
                table: "Advertisements");

            migrationBuilder.DropIndex(
                name: "IX_Advertisements_UserId",
                table: "Advertisements");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Advertisements",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Advertisements",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_UserId1",
                table: "Advertisements",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisements_AspNetUsers_UserId1",
                table: "Advertisements",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
