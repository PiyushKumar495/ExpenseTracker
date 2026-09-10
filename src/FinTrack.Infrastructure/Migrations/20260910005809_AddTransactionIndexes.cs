using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinTrack.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTransactionIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Transactions_IsActive",
                table: "Transactions",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TransactionDate",
                table: "Transactions",
                column: "TransactionDate");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TransactionDirection",
                table: "Transactions",
                column: "TransactionDirection");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TransactionType",
                table: "Transactions",
                column: "TransactionType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Transactions_IsActive",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_TransactionDate",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_TransactionDirection",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_TransactionType",
                table: "Transactions");
        }
    }
}
