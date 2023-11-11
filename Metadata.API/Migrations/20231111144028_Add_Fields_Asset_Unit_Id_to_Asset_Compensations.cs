using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Metadata.API.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldsAssetUnitIdtoAssetCompensations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssetUnitId",
                table: "AssetCompensations",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_AssetCompensations_AssetUnitId",
                table: "AssetCompensations",
                column: "AssetUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssetCompensations_AssetUnits",
                table: "AssetCompensations",
                column: "AssetUnitId",
                principalTable: "AssetUnits",
                principalColumn: "asset_unit_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssetCompensations_AssetUnits",
                table: "AssetCompensations");

            migrationBuilder.DropIndex(
                name: "IX_AssetCompensations_AssetUnitId",
                table: "AssetCompensations");

            migrationBuilder.DropColumn(
                name: "AssetUnitId",
                table: "AssetCompensations");
        }
    }
}
