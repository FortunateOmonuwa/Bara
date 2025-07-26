using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Updateuserdocumentprop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthProfiles_User_UserId",
                table: "AuthProfiles");

            migrationBuilder.DropIndex(
                name: "IX_AuthProfiles_UserId",
                table: "AuthProfiles");

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<Guid>(
                name: "AuthProfileId",
                table: "User",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<long>(
                name: "Size",
                table: "Documents",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_User_AuthProfileId",
                table: "User",
                column: "AuthProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_AuthProfiles_AuthProfileId",
                table: "User",
                column: "AuthProfileId",
                principalTable: "AuthProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_AuthProfiles_AuthProfileId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_AuthProfileId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "AuthProfileId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "Documents");

            migrationBuilder.AlterColumn<int>(
                name: "Gender",
                table: "User",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_AuthProfiles_UserId",
                table: "AuthProfiles",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthProfiles_User_UserId",
                table: "AuthProfiles",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
