using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Metadata.API.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldsToPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "plan_status",
                table: "Plans",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "total_deduction",
                table: "Plans",
                type: "decimal(18,0)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "total_owner_support_compensation",
                table: "Plans",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "total_price_architecture_support_compensation",
                table: "Plans",
                type: "decimal(18,0)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "total_price_compensation",
                table: "Plans",
                type: "decimal(18,0)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "total_price_house_support_compensation",
                table: "Plans",
                type: "decimal(18,0)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "total_price_land_support_compensation",
                table: "Plans",
                type: "decimal(18,0)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "total_price_other_support_compensation",
                table: "Plans",
                type: "decimal(18,0)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "total_price_plant_support_compensation",
                table: "Plans",
                type: "decimal(18,0)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "total_deduction",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "total_owner_support_compensation",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "total_price_architecture_support_compensation",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "total_price_compensation",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "total_price_house_support_compensation",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "total_price_land_support_compensation",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "total_price_other_support_compensation",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "total_price_plant_support_compensation",
                table: "Plans");

            migrationBuilder.AlterColumn<bool>(
                name: "plan_status",
                table: "Plans",
                type: "bit",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);
        }
    }
}
