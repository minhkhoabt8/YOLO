using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Metadata.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSupportTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "support_unit",
                table: "Supports");

            migrationBuilder.AddColumn<string>(
                name: "asset_unit_id",
                table: "Supports",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "support_unit_price",
                table: "Supports",
                type: "decimal(18,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "price_basis",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "land_compensation_basis",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "PriceAppliedCodes",
                type: "bit",
                maxLength: 20,
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Supports_asset_unit_id",
                table: "Supports",
                column: "asset_unit_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Supports_AssetUnits",
                table: "Supports",
                column: "asset_unit_id",
                principalTable: "AssetUnits",
                principalColumn: "asset_unit_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Supports_AssetUnits",
                table: "Supports");

            migrationBuilder.DropIndex(
                name: "IX_Supports_asset_unit_id",
                table: "Supports");

            migrationBuilder.DropColumn(
                name: "asset_unit_id",
                table: "Supports");

            migrationBuilder.DropColumn(
                name: "support_unit_price",
                table: "Supports");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "PriceAppliedCodes");

            migrationBuilder.AddColumn<string>(
                name: "support_unit",
                table: "Supports",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "price_basis",
                table: "Projects",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "land_compensation_basis",
                table: "Projects",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
