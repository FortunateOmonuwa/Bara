using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Chats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScriptId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScriptTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProducerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProducerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WriterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WriterName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScriptTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScriptId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScriptTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProducerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProducerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WriterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WriterName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WriterAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PlatformFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentTransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    WriterPaidAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScriptTransactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "Nvarchar(60)", nullable: false),
                    LastName = table.Column<string>(type: "Nvarchar(60)", nullable: false),
                    MiddleName = table.Column<string>(type: "Nvarchar(60)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bio = table.Column<string>(type: "Nvarchar(200)", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    IsBlacklisted = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    VerificationStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChatMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChatId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SenderName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttachmentUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SentAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatMessages_Chats_ChatId",
                        column: x => x.ChatId,
                        principalTable: "Chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuthProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    IsEmailVerified = table.Column<bool>(type: "bit", nullable: false),
                    LastLoginDevice = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastLoginIPAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LoginAttempts = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastLoginAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LastLogoutAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuthProfiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BankDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountNumber = table.Column<string>(type: "Nvarchar(50)", nullable: false),
                    BankName = table.Column<string>(type: "Nvarchar(100)", nullable: false),
                    BankCode = table.Column<string>(type: "Nvarchar(10)", nullable: false),
                    AccountName = table.Column<string>(type: "Nvarchar(100)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankDetails_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlackListedUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BlackListedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlackListedUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlackListedUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Producers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Producers_Users_Id",
                        column: x => x.Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wallets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LockedBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AvailableBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wallets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Writers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsPremiumMember = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Writers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Writers_Users_Id",
                        column: x => x.Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Fee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CompletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ReferenceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GatewayResponse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WalletID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Wallets_WalletID",
                        column: x => x.WalletID,
                        principalTable: "Wallets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BioExperience",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WriterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Organization = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Project = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    IsCurrent = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BioExperience", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BioExperience_Writers_WriterId",
                        column: x => x.WriterId,
                        principalTable: "Writers",
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
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UploadedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsScriptRegistered = table.Column<bool>(type: "bit", nullable: false),
                    RegistrationBody = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CopyrightNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnershipRights = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ProofUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WriterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WriterName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProducerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scripts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Scripts_Producers_ProducerId",
                        column: x => x.ProducerId,
                        principalTable: "Producers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Scripts_Writers_WriterId",
                        column: x => x.WriterId,
                        principalTable: "Writers",
                        principalColumn: "Id");
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
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IPDealType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SharePercentage = table.Column<int>(type: "int", nullable: false),
                    PaymentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Genre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WriterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Services_Writers_WriterId",
                        column: x => x.WriterId,
                        principalTable: "Writers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EscrowOperations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WalletId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EscrowOperations_Wallets_WalletId",
                        column: x => x.WalletId,
                        principalTable: "Wallets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_UserId",
                table: "Addresses",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuthProfiles_UserId",
                table: "AuthProfiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BankDetails_UserId",
                table: "BankDetails",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BioExperience_WriterId",
                table: "BioExperience",
                column: "WriterId");

            migrationBuilder.CreateIndex(
                name: "IX_BlackListedUsers_UserId",
                table: "BlackListedUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_ChatId",
                table: "ChatMessages",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_UserId",
                table: "Documents",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EscrowOperations_TransactionId",
                table: "EscrowOperations",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_EscrowOperations_WalletId",
                table: "EscrowOperations",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Producers_Id",
                table: "Producers",
                column: "Id",
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
                name: "IX_Services_WriterId",
                table: "Services",
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
                name: "IX_Transactions_WalletID",
                table: "Transactions",
                column: "WalletID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Gender",
                table: "Users",
                column: "Gender");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Id",
                table: "Users",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_IsBlacklisted",
                table: "Users",
                column: "IsBlacklisted");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_UserId",
                table: "Wallets",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Writers_Id",
                table: "Writers",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "AuthProfiles");

            migrationBuilder.DropTable(
                name: "BankDetails");

            migrationBuilder.DropTable(
                name: "BioExperience");

            migrationBuilder.DropTable(
                name: "BlackListedUsers");

            migrationBuilder.DropTable(
                name: "ChatMessages");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "EscrowOperations");

            migrationBuilder.DropTable(
                name: "Scripts");

            migrationBuilder.DropTable(
                name: "ScriptTransactions");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Chats");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Producers");

            migrationBuilder.DropTable(
                name: "Writers");

            migrationBuilder.DropTable(
                name: "Wallets");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
