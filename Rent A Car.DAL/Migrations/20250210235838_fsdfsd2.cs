using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rent_A_Car.DAL.Migrations
{
    /// <inheritdoc />
    public partial class fsdfsd2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "Advertisements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Model",
                table: "Advertisements");
        }
    }
}
