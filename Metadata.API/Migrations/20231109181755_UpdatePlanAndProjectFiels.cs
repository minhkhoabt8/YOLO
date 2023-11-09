using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Metadata.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePlanAndProjectFiels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttachFiles_AssetCompensations",
                table: "AttachFiles");

            migrationBuilder.DropIndex(
                name: "IX_AttachFiles_asset_compensation_id",
                table: "AttachFiles");

            migrationBuilder.DropColumn(
                name: "signer_id",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "plan_location",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "plan_phrase",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "total_price_other_support_compensation",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "asset_compensation_id",
                table: "AttachFiles");

            migrationBuilder.RenameColumn(
                name: "plan_name",
                table: "Plans",
                newName: "plan_report_info");

            migrationBuilder.AddColumn<bool>(
                name: "is_asset_compensation",
                table: "AttachFiles",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_asset_compensation",
                table: "AttachFiles");

            migrationBuilder.RenameColumn(
                name: "plan_report_info",
                table: "Plans",
                newName: "plan_name");

            migrationBuilder.AddColumn<string>(
                name: "signer_id",
                table: "Projects",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "plan_location",
                table: "Plans",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "plan_phrase",
                table: "Plans",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "total_price_other_support_compensation",
                table: "Plans",
                type: "decimal(18,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "asset_compensation_id",
                table: "AttachFiles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AttachFiles_asset_compensation_id",
                table: "AttachFiles",
                column: "asset_compensation_id");

            migrationBuilder.AddForeignKey(
                name: "FK_AttachFiles_AssetCompensations",
                table: "AttachFiles",
                column: "asset_compensation_id",
                principalTable: "AssetCompensations",
                principalColumn: "asset_compensation_id");
        }
    }
}
