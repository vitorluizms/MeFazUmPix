using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyWallet.Migrations
{
    /// <inheritdoc />
    public partial class CorrectionRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Account",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_User",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_PixKeys_User",
                table: "PixKeys");

            migrationBuilder.DropIndex(
                name: "IX_PixKeys_UserId",
                table: "PixKeys");

            migrationBuilder.DropIndex(
                name: "IX_Payments_AccountId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PixKeys");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Payments");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Payments",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Number_Agency",
                table: "Accounts",
                columns: new[] { "Number", "Agency" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_User_UserId",
                table: "Payments",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_User_UserId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_Number_Agency",
                table: "Accounts");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "PixKeys",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Payments",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "Payments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PixKeys_UserId",
                table: "PixKeys",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_AccountId",
                table: "Payments",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Account",
                table: "Payments",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_User",
                table: "Payments",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PixKeys_User",
                table: "PixKeys",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
