using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chirp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FollowCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Follows_AspNetUsers_FollowedById",
                table: "Follows");

            migrationBuilder.AddForeignKey(
                name: "FK_Follows_AspNetUsers_FollowedById",
                table: "Follows",
                column: "FollowedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Follows_AspNetUsers_FollowedById",
                table: "Follows");

            migrationBuilder.AddForeignKey(
                name: "FK_Follows_AspNetUsers_FollowedById",
                table: "Follows",
                column: "FollowedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
