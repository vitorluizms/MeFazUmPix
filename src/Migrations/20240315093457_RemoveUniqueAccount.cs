using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyWallet.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUniqueAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Accounts_UserId_PaymentProviderId",
                table: "Accounts");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_UserId",
                table: "Accounts",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Accounts_UserId",
                table: "Accounts");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_UserId_PaymentProviderId",
                table: "Accounts",
                columns: new[] { "UserId", "PaymentProviderId" },
                unique: true);
        }
    }
}
