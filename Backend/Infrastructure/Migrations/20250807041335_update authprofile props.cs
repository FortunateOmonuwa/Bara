using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateauthprofileprops : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_User_IsBlacklisted",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_IsEmailVerified",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_IsVerified",
                table: "User");

            migrationBuilder.DropColumn(
                name: "IsEmailVerified",
                table: "User");

            migrationBuilder.DropColumn(
                name: "IsVerified",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "User");

            migrationBuilder.AddColumn<string>(
                name: "WriterName",
                table: "Scripts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Documents",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "AuthProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsEmailVerified",
                table: "AuthProfiles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVerified",
                table: "AuthProfiles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastLoginIPAddress",
                table: "AuthProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WriterName",
                table: "Scripts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "AuthProfiles");

            migrationBuilder.DropColumn(
                name: "IsEmailVerified",
                table: "AuthProfiles");

            migrationBuilder.DropColumn(
                name: "IsVerified",
                table: "AuthProfiles");

            migrationBuilder.DropColumn(
                name: "LastLoginIPAddress",
                table: "AuthProfiles");

            migrationBuilder.AddColumn<bool>(
                name: "IsEmailVerified",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVerified",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_User_IsBlacklisted",
                table: "User",
                column: "IsBlacklisted");

            migrationBuilder.CreateIndex(
                name: "IX_User_IsEmailVerified",
                table: "User",
                column: "IsEmailVerified");

            migrationBuilder.CreateIndex(
                name: "IX_User_IsVerified",
                table: "User",
                column: "IsVerified");
        }
    }
}
