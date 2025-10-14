using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chirp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AutoIncrement_AuthorID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "name",
                table: "Authors",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Authors",
                newName: "Email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Authors",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Authors",
                newName: "email");
        }
    }
}
