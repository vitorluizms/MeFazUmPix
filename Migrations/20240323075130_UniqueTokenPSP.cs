using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyWallet.Migrations
{
    /// <inheritdoc />
    public partial class UniqueTokenPSP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PaymentProviders_Token",
                table: "PaymentProviders",
                column: "Token",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PaymentProviders_Token",
                table: "PaymentProviders");
        }
    }
}
