using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chirp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FinalMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cheeps_AspNetUsers_AuthorID",
                table: "Cheeps");

            migrationBuilder.DropForeignKey(
                name: "FK_Follows_AspNetUsers_FollowedById",
                table: "Follows");

            migrationBuilder.DropForeignKey(
                name: "FK_Follows_AspNetUsers_FollowsId",
                table: "Follows");

            migrationBuilder.DropIndex(
                name: "IX_Follows_FollowedById",
                table: "Follows");

            migrationBuilder.AddColumn<string>(
                name: "FollowedByAuthorId",
                table: "Follows",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Follows_FollowedByAuthorId",
                table: "Follows",
                column: "FollowedByAuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cheeps_AspNetUsers_AuthorID",
                table: "Cheeps",
                column: "AuthorID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Follows_AspNetUsers_FollowedByAuthorId",
                table: "Follows",
                column: "FollowedByAuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Follows_AspNetUsers_FollowsId",
                table: "Follows",
                column: "FollowsId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cheeps_AspNetUsers_AuthorID",
                table: "Cheeps");

            migrationBuilder.DropForeignKey(
                name: "FK_Follows_AspNetUsers_FollowedByAuthorId",
                table: "Follows");

            migrationBuilder.DropForeignKey(
                name: "FK_Follows_AspNetUsers_FollowsId",
                table: "Follows");

            migrationBuilder.DropIndex(
                name: "IX_Follows_FollowedByAuthorId",
                table: "Follows");

            migrationBuilder.DropColumn(
                name: "FollowedByAuthorId",
                table: "Follows");

            migrationBuilder.CreateIndex(
                name: "IX_Follows_FollowedById",
                table: "Follows",
                column: "FollowedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Cheeps_AspNetUsers_AuthorID",
                table: "Cheeps",
                column: "AuthorID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Follows_AspNetUsers_FollowedById",
                table: "Follows",
                column: "FollowedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Follows_AspNetUsers_FollowsId",
                table: "Follows",
                column: "FollowsId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
