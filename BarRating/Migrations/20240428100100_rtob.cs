using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarRating.Migrations
{
    /// <inheritdoc />
    public partial class rtob : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BarId",
                table: "Reviews",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_BarId",
                table: "Reviews",
                column: "BarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Bars_BarId",
                table: "Reviews",
                column: "BarId",
                principalTable: "Bars",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Bars_BarId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_BarId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "BarId",
                table: "Reviews");
        }
    }
}
