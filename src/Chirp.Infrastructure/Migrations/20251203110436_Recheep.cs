using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chirp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Recheep : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Recheeps",
                columns: table => new
                {
                    AuthorID = table.Column<string>(type: "TEXT", nullable: false),
                    CheepID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recheeps", x => new { x.AuthorID, x.CheepID });
                    table.ForeignKey(
                        name: "FK_Recheeps_AspNetUsers_AuthorID",
                        column: x => x.AuthorID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Recheeps");
        }
    }
}
