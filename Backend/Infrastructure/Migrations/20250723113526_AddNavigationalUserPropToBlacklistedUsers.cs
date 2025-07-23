using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNavigationalUserPropToBlacklistedUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_BlackListedUsers_UserId",
                table: "BlackListedUsers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlackListedUsers_User_UserId",
                table: "BlackListedUsers",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlackListedUsers_User_UserId",
                table: "BlackListedUsers");

            migrationBuilder.DropIndex(
                name: "IX_BlackListedUsers_UserId",
                table: "BlackListedUsers");
        }
    }
}
