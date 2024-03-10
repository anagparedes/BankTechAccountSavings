using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankTechAccountSavings.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class BankTechAccountSaving : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountSavings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    AccountNumber = table.Column<long>(type: "bigint", nullable: false),
                    AccountName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bank = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MonthlyInterestGenerated = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AnnualInterestProjected = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AnnualInterestRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MonthlyInterestRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    DateOpened = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateClosed = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AccountStatus = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountSavings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConfirmationNumber = table.Column<int>(type: "int", nullable: false),
                    Voucher = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionType = table.Column<int>(type: "int", nullable: false),
                    TransactionStatus = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    DestinationProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DestinationProductNumber = table.Column<long>(type: "bigint", nullable: true),
                    Credit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SourceProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Transfer_DestinationProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Transfer_DestinationProductNumber = table.Column<long>(type: "bigint", nullable: true),
                    SourceProductNumber = table.Column<long>(type: "bigint", nullable: true),
                    TransferType = table.Column<int>(type: "int", nullable: true),
                    Commission = table.Column<int>(type: "int", nullable: true),
                    Tax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Transfer_Credit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Debit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Withdraw_SourceProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Withdraw_SourceProductNumber = table.Column<long>(type: "bigint", nullable: true),
                    Withdraw_Debit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Withdraw_Tax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Withdraw_Total = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_AccountSavings_DestinationProductId",
                        column: x => x.DestinationProductId,
                        principalTable: "AccountSavings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_AccountSavings_SourceProductId",
                        column: x => x.SourceProductId,
                        principalTable: "AccountSavings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_AccountSavings_Transfer_DestinationProductId",
                        column: x => x.Transfer_DestinationProductId,
                        principalTable: "AccountSavings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_AccountSavings_Withdraw_SourceProductId",
                        column: x => x.Withdraw_SourceProductId,
                        principalTable: "AccountSavings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_DestinationProductId",
                table: "Transactions",
                column: "DestinationProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_SourceProductId",
                table: "Transactions",
                column: "SourceProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_Transfer_DestinationProductId",
                table: "Transactions",
                column: "Transfer_DestinationProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_Withdraw_SourceProductId",
                table: "Transactions",
                column: "Withdraw_SourceProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "AccountSavings");
        }
    }
}
