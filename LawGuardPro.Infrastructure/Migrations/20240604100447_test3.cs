using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LawGuardPro.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class test3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cases_AspNetUsers_ApplicationUserId",
                table: "Cases");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "Cases",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Cases_ApplicationUserId",
                table: "Cases",
                newName: "IX_Cases_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_AspNetUsers_UserId",
                table: "Cases",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cases_AspNetUsers_UserId",
                table: "Cases");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Cases",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Cases_UserId",
                table: "Cases",
                newName: "IX_Cases_ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_AspNetUsers_ApplicationUserId",
                table: "Cases",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
