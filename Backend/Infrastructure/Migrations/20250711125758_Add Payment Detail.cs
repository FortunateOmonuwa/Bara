using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PaymentDetailId",
                table: "Transactions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WalletID",
                table: "Transactions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PaymentDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentMethodToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CardBrand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Last4 = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentDetail", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_PaymentDetailId",
                table: "Transactions",
                column: "PaymentDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_WalletID",
                table: "Transactions",
                column: "WalletID");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_PaymentDetail_PaymentDetailId",
                table: "Transactions",
                column: "PaymentDetailId",
                principalTable: "PaymentDetail",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Wallets_WalletID",
                table: "Transactions",
                column: "WalletID",
                principalTable: "Wallets",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_PaymentDetail_PaymentDetailId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Wallets_WalletID",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "PaymentDetail");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_PaymentDetailId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_WalletID",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "PaymentDetailId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "WalletID",
                table: "Transactions");
        }
    }
}
