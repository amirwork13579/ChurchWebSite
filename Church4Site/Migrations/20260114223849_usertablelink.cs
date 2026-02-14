using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Church4Site.Migrations
{
    /// <inheritdoc />
    public partial class usertablelink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserMessages_UserId",
                table: "UserMessages",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserMessages_Users_UserId",
                table: "UserMessages",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserMessages_Users_UserId",
                table: "UserMessages");

            migrationBuilder.DropIndex(
                name: "IX_UserMessages_UserId",
                table: "UserMessages");
        }
    }
}
