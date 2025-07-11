using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _2ndInitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProofOfAddressDocument = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdentificationNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DocumentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    DocumentUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileExtension = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Wallets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LockedBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "Nvarchar(60)", nullable: false),
                    LastName = table.Column<string>(type: "Nvarchar(60)", nullable: false),
                    MiddleName = table.Column<string>(type: "Nvarchar(60)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bio = table.Column<string>(type: "Nvarchar(200)", nullable: false),
                    IsEmailVerified = table.Column<bool>(type: "bit", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VerificationDocumentID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    IsBlacklisted = table.Column<bool>(type: "bit", nullable: false),
                    WalletId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_User_Documents_VerificationDocumentID",
                        column: x => x.VerificationDocumentID,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_User_Wallets_WalletId",
                        column: x => x.WalletId,
                        principalTable: "Wallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuthProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastLoginAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastLogoutAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuthProfiles_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Scripts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "Nvarchar(100)", maxLength: 200, nullable: false),
                    Genre = table.Column<string>(type: "Nvarchar(60)", maxLength: 100, nullable: false),
                    Logline = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Synopsis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsScriptRegistered = table.Column<bool>(type: "bit", nullable: false),
                    RegistrationBody = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CopyrightNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnershipRights = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProofUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WriterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProducerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scripts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Scripts_User_ProducerId",
                        column: x => x.ProducerId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Scripts_User_WriterId",
                        column: x => x.WriterId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "Nvarchar(60)", nullable: false),
                    Description = table.Column<string>(type: "Nvarchar(200)", nullable: false),
                    MinPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IPDealType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SharePercentage = table.Column<int>(type: "int", nullable: false),
                    PaymentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Genre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScriptWriterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Services_User_ScriptWriterId",
                        column: x => x.ScriptWriterId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProducerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WriterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScriptId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CompletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ReferenceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GatewayResponse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id", onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ScriptPDFs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScriptID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileExtension = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UploadedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "SharedScripts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScriptId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WriterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProducerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SharedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    EncryptedScriptUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharedScripts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SharedScripts_Scripts_ScriptId",
                        column: x => x.ScriptId,
                        principalTable: "Scripts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_SharedScripts_User_ProducerId",
                        column: x => x.ProducerId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SharedScripts_User_WriterId",
                        column: x => x.WriterId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EscrowOperations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LockedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ReleasedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EscrowOperations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EscrowOperations_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "Writer" },
                    { 3, "Producer" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthProfiles_UserId",
                table: "AuthProfiles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EscrowOperations_TransactionId",
                table: "EscrowOperations",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_ScriptPDFs_ScriptID",
                table: "ScriptPDFs",
                column: "ScriptID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Scripts_Genre",
                table: "Scripts",
                column: "Genre");

            migrationBuilder.CreateIndex(
                name: "IX_Scripts_ProducerId",
                table: "Scripts",
                column: "ProducerId");

            migrationBuilder.CreateIndex(
                name: "IX_Scripts_Status",
                table: "Scripts",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Scripts_Title",
                table: "Scripts",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_Scripts_WriterId",
                table: "Scripts",
                column: "WriterId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_ScriptWriterId",
                table: "Services",
                column: "ScriptWriterId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedScripts_ProducerId",
                table: "SharedScripts",
                column: "ProducerId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedScripts_ScriptId",
                table: "SharedScripts",
                column: "ScriptId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedScripts_WriterId",
                table: "SharedScripts",
                column: "WriterId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_Status",
                table: "Transactions",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TransactionType",
                table: "Transactions",
                column: "TransactionType");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_UserId",
                table: "Transactions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_User_AddressId",
                table: "User",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_IsBlacklisted",
                table: "User",
                column: "IsBlacklisted");

            migrationBuilder.CreateIndex(
                name: "IX_User_IsDeleted",
                table: "User",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_User_IsEmailVerified",
                table: "User",
                column: "IsEmailVerified");

            migrationBuilder.CreateIndex(
                name: "IX_User_IsVerified",
                table: "User",
                column: "IsVerified");

            migrationBuilder.CreateIndex(
                name: "IX_User_VerificationDocumentID",
                table: "User",
                column: "VerificationDocumentID");

            migrationBuilder.CreateIndex(
                name: "IX_User_WalletId",
                table: "User",
                column: "WalletId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthProfiles");

            migrationBuilder.DropTable(
                name: "EscrowOperations");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "ScriptPDFs");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "SharedScripts");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Scripts");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "Wallets");
        }
    }
}
