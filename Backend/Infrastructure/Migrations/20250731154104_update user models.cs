using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateusermodels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScriptPDFs");

            migrationBuilder.AlterColumn<string>(
                name: "OwnershipRights",
                table: "Scripts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "Scripts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UploadedOn",
                table: "Scripts",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Scripts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "LastLoginAt",
                table: "AuthProfiles",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AuthProfiles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "AuthProfiles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastLoginDevice",
                table: "AuthProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "LoginAttempts",
                table: "AuthProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "AuthProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path",
                table: "Scripts");

            migrationBuilder.DropColumn(
                name: "UploadedOn",
                table: "Scripts");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Scripts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AuthProfiles");

            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "AuthProfiles");

            migrationBuilder.DropColumn(
                name: "LastLoginDevice",
                table: "AuthProfiles");

            migrationBuilder.DropColumn(
                name: "LoginAttempts",
                table: "AuthProfiles");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "AuthProfiles");

            migrationBuilder.AlterColumn<string>(
                name: "OwnershipRights",
                table: "Scripts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "LastLoginAt",
                table: "AuthProfiles",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ScriptPDFs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScriptID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UploadedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScriptPDFs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScriptPDFs_Scripts_ScriptID",
                        column: x => x.ScriptID,
                        principalTable: "Scripts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScriptPDFs_ScriptID",
                table: "ScriptPDFs",
                column: "ScriptID",
                unique: true);
        }
    }
}
