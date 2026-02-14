using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Church4Site.Migrations
{
    /// <inheritdoc />
    public partial class RenameEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "TeamMembers",
                newName: "Email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Email",
                table: "TeamMembers",
                newName: "Name");
        }
    }
}
