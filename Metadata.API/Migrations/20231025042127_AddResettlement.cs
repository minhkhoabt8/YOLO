using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Metadata.API.Migrations
{
    /// <inheritdoc />
    public partial class AddResettlement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AuditTrails",
                table: "AuditTrails");

            migrationBuilder.RenameColumn(
                name: "land_position_rest",
                table: "UnitPriceLands",
                newName: "land_position_5");

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "UnitPriceLands",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "UnitPriceAssets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "SupportTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "plan_approved_by",
                table: "Plans",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nchar(10)",
                oldFixedLength: true,
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "OrganizationTypes",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "code",
                table: "OrganizationTypes",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "OrganizationTypes",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "MeasuredLandInfo",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "LandTypes",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "code",
                table: "LandTypes",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "LandTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "LandGroups",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "code",
                table: "LandGroups",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "LandGroups",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "GCNLandInfos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "DocumentTypes",
                type: "bit",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "published_date",
                table: "Documents",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "number",
                table: "Documents",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "notation",
                table: "Documents",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<DateTime>(
                name: "effective_date",
                table: "Documents",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_time",
                table: "Documents",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<string>(
                name: "created_by",
                table: "Documents",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "DeductionTypes",
                type: "bit",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "user_name",
                table: "AuditTrails",
                type: "nvarchar(max)",
                nullable: true,
                collation: "SQL_Latin1_General_CP1_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "user_id",
                table: "AuditTrails",
                type: "nvarchar(max)",
                nullable: true,
                collation: "SQL_Latin1_General_CP1_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "type",
                table: "AuditTrails",
                type: "nvarchar(max)",
                nullable: true,
                collation: "SQL_Latin1_General_CP1_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "table_name",
                table: "AuditTrails",
                type: "nvarchar(max)",
                nullable: false,
                collation: "SQL_Latin1_General_CP1_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "primary_key",
                table: "AuditTrails",
                type: "nvarchar(max)",
                nullable: false,
                collation: "SQL_Latin1_General_CP1_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "old_value",
                table: "AuditTrails",
                type: "nvarchar(max)",
                nullable: true,
                collation: "SQL_Latin1_General_CP1_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "new_value",
                table: "AuditTrails",
                type: "nvarchar(max)",
                nullable: false,
                collation: "SQL_Latin1_General_CP1_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "affected_column",
                table: "AuditTrails",
                type: "nvarchar(max)",
                nullable: true,
                collation: "SQL_Latin1_General_CP1_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "id",
                table: "AuditTrails",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                collation: "SQL_Latin1_General_CP1_CI_AS",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "AssetUnits",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "AssetGroups",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "AssetCompensations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuditTrails_1",
                table: "AuditTrails",
                column: "id");

            migrationBuilder.CreateTable(
                name: "ResettlementProjects",
                columns: table => new
                {
                    resettlementprojectid = table.Column<string>(name: "resettlement_project_id", type: "nvarchar(50)", maxLength: 50, nullable: false),
                    code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    limittoresettlement = table.Column<string>(name: "limit_to_resettlement", type: "nvarchar(50)", maxLength: 50, nullable: true),
                    limittoconsideration = table.Column<string>(name: "limit_to_consideration", type: "nvarchar(50)", maxLength: 50, nullable: true),
                    position = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    landnumber = table.Column<string>(name: "land_number", type: "nvarchar(10)", maxLength: 10, nullable: true),
                    implementyear = table.Column<string>(name: "implement_year", type: "nvarchar(4)", maxLength: 4, nullable: true),
                    landprice = table.Column<decimal>(name: "land_price", type: "decimal(18,0)", nullable: true),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lastdateedit = table.Column<DateTime>(name: "last_date_edit", type: "datetime", nullable: true),
                    lastpersonedit = table.Column<string>(name: "last_person_edit", type: "nvarchar(50)", maxLength: 50, nullable: true),
                    documentid = table.Column<string>(name: "document_id", type: "nvarchar(50)", maxLength: 50, nullable: true),
                    projectid = table.Column<string>(name: "project_id", type: "nvarchar(50)", maxLength: 50, nullable: true),
                    isdeleted = table.Column<bool>(name: "is_deleted", type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResettlementProjects", x => x.resettlementprojectid);
                    table.ForeignKey(
                        name: "FK_ResettlementProjects_Projects",
                        column: x => x.projectid,
                        principalTable: "Projects",
                        principalColumn: "project_id");
                });

            migrationBuilder.CreateTable(
                name: "LandResettlements",
                columns: table => new
                {
                    landresettlementid = table.Column<string>(name: "land_resettlement_id", type: "nvarchar(50)", maxLength: 50, nullable: false),
                    position = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    plotnumber = table.Column<string>(name: "plot_number", type: "nvarchar(10)", maxLength: 10, nullable: true),
                    pagenumber = table.Column<string>(name: "page_number", type: "nvarchar(10)", maxLength: 10, nullable: true),
                    plotaddress = table.Column<string>(name: "plot_address", type: "nvarchar(100)", maxLength: 100, nullable: true),
                    landsize = table.Column<decimal>(name: "land_size", type: "decimal(18,0)", nullable: true),
                    totallandprice = table.Column<decimal>(name: "total_land_price", type: "decimal(18,0)", nullable: true),
                    resettlementprojectid = table.Column<string>(name: "resettlement_project_id", type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ownerid = table.Column<string>(name: "owner_id", type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandResettlements", x => x.landresettlementid);
                    table.ForeignKey(
                        name: "FK_LandResettlements_Owners",
                        column: x => x.ownerid,
                        principalTable: "Owners",
                        principalColumn: "owner_id");
                    table.ForeignKey(
                        name: "FK_LandResettlements_ResettlementProjects",
                        column: x => x.resettlementprojectid,
                        principalTable: "ResettlementProjects",
                        principalColumn: "resettlement_project_id");
                });

            migrationBuilder.CreateTable(
                name: "ResettlementDocuments",
                columns: table => new
                {
                    projectdocumentid = table.Column<string>(name: "project_document_id", type: "nvarchar(50)", maxLength: 50, nullable: false),
                    resettlementprojectid = table.Column<string>(name: "resettlement_project_id", type: "nvarchar(50)", maxLength: 50, nullable: true),
                    documentid = table.Column<string>(name: "document_id", type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResettlementDocuments", x => x.projectdocumentid);
                    table.ForeignKey(
                        name: "FK_ResettlementDocuments_Documents",
                        column: x => x.documentid,
                        principalTable: "Documents",
                        principalColumn: "document_id");
                    table.ForeignKey(
                        name: "FK_ResettlementDocuments_ResettlementProjects",
                        column: x => x.resettlementprojectid,
                        principalTable: "ResettlementProjects",
                        principalColumn: "resettlement_project_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LandResettlements_owner_id",
                table: "LandResettlements",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_LandResettlements_resettlement_project_id",
                table: "LandResettlements",
                column: "resettlement_project_id");

            migrationBuilder.CreateIndex(
                name: "IX_ResettlementDocuments_document_id",
                table: "ResettlementDocuments",
                column: "document_id");

            migrationBuilder.CreateIndex(
                name: "IX_ResettlementDocuments_resettlement_project_id",
                table: "ResettlementDocuments",
                column: "resettlement_project_id");

            migrationBuilder.CreateIndex(
                name: "IX_ResettlementProjects_project_id",
                table: "ResettlementProjects",
                column: "project_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LandResettlements");

            migrationBuilder.DropTable(
                name: "ResettlementDocuments");

            migrationBuilder.DropTable(
                name: "ResettlementProjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AuditTrails_1",
                table: "AuditTrails");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "UnitPriceLands");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "UnitPriceAssets");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "SupportTypes");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "OrganizationTypes");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "MeasuredLandInfo");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "LandTypes");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "LandGroups");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "GCNLandInfos");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "DocumentTypes");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "DeductionTypes");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "AssetUnits");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "AssetGroups");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "AssetCompensations");

            migrationBuilder.RenameColumn(
                name: "land_position_5",
                table: "UnitPriceLands",
                newName: "land_position_rest");

            migrationBuilder.AlterColumn<string>(
                name: "plan_approved_by",
                table: "Plans",
                type: "nchar(10)",
                fixedLength: true,
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "OrganizationTypes",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "code",
                table: "OrganizationTypes",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "LandTypes",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "code",
                table: "LandTypes",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "LandGroups",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "code",
                table: "LandGroups",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<DateTime>(
                name: "published_date",
                table: "Documents",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "number",
                table: "Documents",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "notation",
                table: "Documents",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "effective_date",
                table: "Documents",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_time",
                table: "Documents",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "created_by",
                table: "Documents",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "user_name",
                table: "AuditTrails",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldCollation: "SQL_Latin1_General_CP1_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "user_id",
                table: "AuditTrails",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldCollation: "SQL_Latin1_General_CP1_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "type",
                table: "AuditTrails",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldCollation: "SQL_Latin1_General_CP1_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "table_name",
                table: "AuditTrails",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldCollation: "SQL_Latin1_General_CP1_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "primary_key",
                table: "AuditTrails",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldCollation: "SQL_Latin1_General_CP1_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "old_value",
                table: "AuditTrails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldCollation: "SQL_Latin1_General_CP1_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "new_value",
                table: "AuditTrails",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldCollation: "SQL_Latin1_General_CP1_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "affected_column",
                table: "AuditTrails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldCollation: "SQL_Latin1_General_CP1_CI_AS");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "AuditTrails",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldCollation: "SQL_Latin1_General_CP1_CI_AS");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuditTrails",
                table: "AuditTrails",
                column: "id");
        }
    }
}
