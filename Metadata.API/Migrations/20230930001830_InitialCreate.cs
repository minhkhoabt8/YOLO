using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Metadata.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssetGroups",
                columns: table => new
                {
                    assetgroupid = table.Column<string>(name: "asset_group_id", type: "nvarchar(50)", maxLength: 50, nullable: false),
                    code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetGroups", x => x.assetgroupid);
                });

            migrationBuilder.CreateTable(
                name: "AssetUnits",
                columns: table => new
                {
                    assetunitid = table.Column<string>(name: "asset_unit_id", type: "nvarchar(50)", maxLength: 50, nullable: false),
                    code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetUnits", x => x.assetunitid);
                });

            migrationBuilder.CreateTable(
                name: "DeductionTypes",
                columns: table => new
                {
                    deductiontypeid = table.Column<string>(name: "deduction_type_id", type: "nvarchar(50)", maxLength: 50, nullable: false),
                    code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeductionTypes", x => x.deductiontypeid);
                });

            migrationBuilder.CreateTable(
                name: "DocumentTypes",
                columns: table => new
                {
                    documenttypeid = table.Column<string>(name: "document_type_id", type: "nvarchar(50)", maxLength: 50, nullable: false),
                    code = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTypes", x => x.documenttypeid);
                });

            migrationBuilder.CreateTable(
                name: "LandGroups",
                columns: table => new
                {
                    landgroupid = table.Column<string>(name: "land_group_id", type: "nvarchar(50)", maxLength: 50, nullable: false),
                    code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandGroups", x => x.landgroupid);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationTypes",
                columns: table => new
                {
                    organizationtypeid = table.Column<string>(name: "organization_type_id", type: "nvarchar(50)", maxLength: 50, nullable: false),
                    code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationTypes", x => x.organizationtypeid);
                });

            migrationBuilder.CreateTable(
                name: "PriceAppliedCodes",
                columns: table => new
                {
                    priceappliedcodeid = table.Column<string>(name: "price_applied_code_id", type: "nvarchar(50)", maxLength: 50, nullable: false),
                    unitpricecode = table.Column<string>(name: "unit_price_code", type: "nvarchar(20)", maxLength: 20, nullable: true),
                    pricecontent = table.Column<string>(name: "price_content", type: "ntext", nullable: true),
                    expriredtime = table.Column<DateTime>(name: "exprired_time", type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceAppliedCode", x => x.priceappliedcodeid);
                });

            migrationBuilder.CreateTable(
                name: "SupportTypes",
                columns: table => new
                {
                    supporttypeid = table.Column<string>(name: "support_type_id", type: "nvarchar(50)", maxLength: 50, nullable: false),
                    code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportType", x => x.supporttypeid);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    documentid = table.Column<string>(name: "document_id", type: "nvarchar(50)", maxLength: 50, nullable: false),
                    documenttypeid = table.Column<string>(name: "document_type_id", type: "nvarchar(50)", maxLength: 50, nullable: true),
                    number = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    notation = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    createdby = table.Column<string>(name: "created_by", type: "nvarchar(20)", maxLength: 20, nullable: true),
                    createdtime = table.Column<DateTime>(name: "created_time", type: "datetime", nullable: true),
                    publisheddate = table.Column<DateTime>(name: "published_date", type: "date", nullable: true),
                    effectivedate = table.Column<DateTime>(name: "effective_date", type: "date", nullable: true),
                    epitome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    signinfo = table.Column<string>(name: "sign_info", type: "nvarchar(max)", nullable: true),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pen = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    referencelink = table.Column<string>(name: "reference_link", type: "nvarchar(max)", nullable: true),
                    ispublic = table.Column<bool>(name: "is_public", type: "bit", nullable: true),
                    isdeleted = table.Column<bool>(name: "is_deleted", type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.documentid);
                    table.ForeignKey(
                        name: "FK_Documents_DocumentTypes",
                        column: x => x.documenttypeid,
                        principalTable: "DocumentTypes",
                        principalColumn: "document_type_id");
                });

            migrationBuilder.CreateTable(
                name: "LandTypes",
                columns: table => new
                {
                    landtypeid = table.Column<string>(name: "land_type_id", type: "nvarchar(50)", maxLength: 50, nullable: false),
                    code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    landgroupid = table.Column<string>(name: "land_group_id", type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandTypes", x => x.landtypeid);
                    table.ForeignKey(
                        name: "FK_LandTypes_LandGroups",
                        column: x => x.landgroupid,
                        principalTable: "LandGroups",
                        principalColumn: "land_group_id");
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    projectid = table.Column<string>(name: "project_id", type: "nvarchar(50)", maxLength: 50, nullable: false),
                    projectcode = table.Column<string>(name: "project_code", type: "nvarchar(50)", maxLength: 50, nullable: true),
                    projectname = table.Column<string>(name: "project_name", type: "nvarchar(50)", maxLength: 50, nullable: true),
                    projectlocation = table.Column<string>(name: "project_location", type: "nvarchar(200)", maxLength: 200, nullable: true),
                    province = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    district = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ward = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    projectexpense = table.Column<decimal>(name: "project_expense", type: "decimal(18,0)", nullable: true),
                    projectapprovaldate = table.Column<DateTime>(name: "project_approval_date", type: "date", nullable: true),
                    implementationyear = table.Column<string>(name: "implementation_year", type: "nvarchar(4)", maxLength: 4, nullable: true),
                    regulatedunitprice = table.Column<string>(name: "regulated_unit_price", type: "nvarchar(20)", maxLength: 20, nullable: true),
                    projectbriefnumber = table.Column<string>(name: "project_brief_number", type: "nvarchar(10)", maxLength: 10, nullable: true),
                    projectnote = table.Column<string>(name: "project_note", type: "nvarchar(50)", maxLength: 50, nullable: true),
                    priceappliedcodeid = table.Column<string>(name: "price_applied_code_id", type: "nvarchar(50)", maxLength: 50, nullable: true),
                    checkcode = table.Column<string>(name: "check_code", type: "nvarchar(20)", maxLength: 20, nullable: true),
                    reportsignal = table.Column<string>(name: "report_signal", type: "ntext", nullable: true),
                    reportnumber = table.Column<string>(name: "report_number", type: "nvarchar(20)", maxLength: 20, nullable: true),
                    pricebasis = table.Column<string>(name: "price_basis", type: "nvarchar(20)", maxLength: 20, nullable: true),
                    landcompensationbasis = table.Column<string>(name: "land_compensation_basis", type: "nvarchar(20)", maxLength: 20, nullable: true),
                    assetcompensationbasis = table.Column<string>(name: "asset_compensation_basis", type: "nvarchar(20)", maxLength: 20, nullable: true),
                    projectcreatedtime = table.Column<DateTime>(name: "project_created_time", type: "date", nullable: true),
                    projectcreatedby = table.Column<string>(name: "project_created_by", type: "nvarchar(20)", maxLength: 20, nullable: true),
                    projectstatus = table.Column<bool>(name: "project_status", type: "bit", nullable: true),
                    isdeleted = table.Column<bool>(name: "is_deleted", type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.projectid);
                    table.ForeignKey(
                        name: "FK_Projects_PriceAppliedCodes",
                        column: x => x.priceappliedcodeid,
                        principalTable: "PriceAppliedCodes",
                        principalColumn: "price_applied_code_id");
                });

            migrationBuilder.CreateTable(
                name: "UnitPriceAssets",
                columns: table => new
                {
                    unitpriceassetid = table.Column<string>(name: "unit_price_asset_id", type: "nvarchar(50)", maxLength: 50, nullable: false),
                    assetname = table.Column<string>(name: "asset_name", type: "nvarchar(20)", maxLength: 20, nullable: true),
                    assetprice = table.Column<decimal>(name: "asset_price", type: "decimal(10,3)", nullable: true),
                    assetregulation = table.Column<string>(name: "asset_regulation", type: "nvarchar(20)", maxLength: 20, nullable: true),
                    assettype = table.Column<string>(name: "asset_type", type: "nvarchar(20)", maxLength: 20, nullable: true),
                    priceappliedcodeid = table.Column<string>(name: "price_applied_code_id", type: "nvarchar(50)", maxLength: 50, nullable: true),
                    assetunitid = table.Column<string>(name: "asset_unit_id", type: "nvarchar(50)", maxLength: 50, nullable: true),
                    assetgroupid = table.Column<string>(name: "asset_group_id", type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitPriceAssets", x => x.unitpriceassetid);
                    table.ForeignKey(
                        name: "FK_UnitPriceAssets_AssetGroups",
                        column: x => x.assetgroupid,
                        principalTable: "AssetGroups",
                        principalColumn: "asset_group_id");
                    table.ForeignKey(
                        name: "FK_UnitPriceAssets_AssetUnits",
                        column: x => x.assetunitid,
                        principalTable: "AssetUnits",
                        principalColumn: "asset_unit_id");
                    table.ForeignKey(
                        name: "FK_UnitPriceAssets_PriceAppliedCodes",
                        column: x => x.priceappliedcodeid,
                        principalTable: "PriceAppliedCodes",
                        principalColumn: "price_applied_code_id");
                });

            migrationBuilder.CreateTable(
                name: "LandPositionInfos",
                columns: table => new
                {
                    landinfopositionid = table.Column<string>(name: "land_info_position_id", type: "nvarchar(50)", maxLength: 50, nullable: false),
                    locationname = table.Column<string>(name: "location_name", type: "nvarchar(50)", maxLength: 50, nullable: true),
                    description = table.Column<string>(type: "ntext", nullable: true),
                    projectid = table.Column<string>(name: "project_id", type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandPositionInfos", x => x.landinfopositionid);
                    table.ForeignKey(
                        name: "FK_LandPositionInfos_Projects",
                        column: x => x.projectid,
                        principalTable: "Projects",
                        principalColumn: "project_id");
                });

            migrationBuilder.CreateTable(
                name: "Plans",
                columns: table => new
                {
                    planid = table.Column<string>(name: "plan_id", type: "nvarchar(50)", maxLength: 50, nullable: false),
                    projectid = table.Column<string>(name: "project_id", type: "nvarchar(50)", maxLength: 50, nullable: true),
                    planecode = table.Column<string>(name: "plane_code", type: "nvarchar(10)", maxLength: 10, nullable: true),
                    planphrase = table.Column<string>(name: "plan_phrase", type: "nvarchar(10)", maxLength: 10, nullable: true),
                    plandescription = table.Column<string>(name: "plan_description", type: "ntext", nullable: true),
                    plancreatebase = table.Column<string>(name: "plan_create_base", type: "nvarchar(50)", maxLength: 50, nullable: true),
                    planapprovedby = table.Column<string>(name: "plan_approved_by", type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true),
                    planreportsignal = table.Column<string>(name: "plan_report_signal", type: "ntext", nullable: true),
                    planreportdate = table.Column<DateTime>(name: "plan_report_date", type: "date", nullable: true),
                    plancreatedtime = table.Column<DateTime>(name: "plan_created_time", type: "datetime", nullable: true),
                    planendedtime = table.Column<DateTime>(name: "plan_ended_time", type: "datetime", nullable: true),
                    plancreatedby = table.Column<string>(name: "plan_created_by", type: "nvarchar(20)", maxLength: 20, nullable: true),
                    planstatus = table.Column<bool>(name: "plan_status", type: "bit", nullable: true),
                    isdeleted = table.Column<bool>(name: "is_deleted", type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plans", x => x.planid);
                    table.ForeignKey(
                        name: "FK_Plans_Projects",
                        column: x => x.projectid,
                        principalTable: "Projects",
                        principalColumn: "project_id");
                });

            migrationBuilder.CreateTable(
                name: "ProjectDocuments",
                columns: table => new
                {
                    projectdocumentid = table.Column<string>(name: "project_document_id", type: "nvarchar(50)", maxLength: 50, nullable: false),
                    projectid = table.Column<string>(name: "project_id", type: "nvarchar(50)", maxLength: 50, nullable: true),
                    documentid = table.Column<string>(name: "document_id", type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectDocuments", x => x.projectdocumentid);
                    table.ForeignKey(
                        name: "FK_ProjectDocuments_Documents",
                        column: x => x.documentid,
                        principalTable: "Documents",
                        principalColumn: "document_id");
                    table.ForeignKey(
                        name: "FK_ProjectDocuments_Projects",
                        column: x => x.projectid,
                        principalTable: "Projects",
                        principalColumn: "project_id");
                });

            migrationBuilder.CreateTable(
                name: "UnitPriceLands",
                columns: table => new
                {
                    unitpricelandid = table.Column<string>(name: "unit_price_land_id", type: "nvarchar(50)", maxLength: 50, nullable: false),
                    projectid = table.Column<string>(name: "project_id", type: "nvarchar(50)", maxLength: 50, nullable: true),
                    streetareaname = table.Column<string>(name: "street_area_name", type: "nvarchar(50)", maxLength: 50, nullable: true),
                    landtypeid = table.Column<string>(name: "land_type_id", type: "nvarchar(50)", maxLength: 50, nullable: true),
                    landunit = table.Column<string>(name: "land_unit", type: "nvarchar(20)", maxLength: 20, nullable: true),
                    landposition1 = table.Column<decimal>(name: "land_position_1", type: "decimal(10,3)", nullable: true),
                    landposition2 = table.Column<decimal>(name: "land_position_2", type: "decimal(10,3)", nullable: true),
                    landposition3 = table.Column<decimal>(name: "land_position_3", type: "decimal(10,3)", nullable: true),
                    landposition4 = table.Column<decimal>(name: "land_position_4", type: "decimal(10,3)", nullable: true),
                    landpositionrest = table.Column<decimal>(name: "land_position_rest", type: "decimal(10,3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitPriceLands", x => x.unitpricelandid);
                    table.ForeignKey(
                        name: "FK_UnitPriceLands_LandTypes",
                        column: x => x.landtypeid,
                        principalTable: "LandTypes",
                        principalColumn: "land_type_id");
                    table.ForeignKey(
                        name: "FK_UnitPriceLands_Projects",
                        column: x => x.projectid,
                        principalTable: "Projects",
                        principalColumn: "project_id");
                });

            migrationBuilder.CreateTable(
                name: "Owners",
                columns: table => new
                {
                    ownerid = table.Column<string>(name: "owner_id", type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ownercode = table.Column<string>(name: "owner_code", type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ownername = table.Column<string>(name: "owner_name", type: "nvarchar(50)", maxLength: 50, nullable: true),
                    owneridcode = table.Column<string>(name: "owner_id_code", type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ownergender = table.Column<string>(name: "owner_gender", type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ownerdateofbirth = table.Column<DateTime>(name: "owner_date_of_birth", type: "date", nullable: true),
                    ownerethnic = table.Column<string>(name: "owner_ethnic", type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ownernational = table.Column<string>(name: "owner_national", type: "nvarchar(50)", maxLength: 50, nullable: true),
                    owneraddress = table.Column<string>(name: "owner_address", type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ownertaxcode = table.Column<string>(name: "owner_tax_code", type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ownertype = table.Column<string>(name: "owner_type", type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ownercreatedtime = table.Column<DateTime>(name: "owner_created_time", type: "datetime", nullable: true),
                    ownercreatedby = table.Column<string>(name: "owner_created_by", type: "nvarchar(20)", maxLength: 20, nullable: true),
                    projectid = table.Column<string>(name: "project_id", type: "nvarchar(50)", maxLength: 50, nullable: true),
                    planid = table.Column<string>(name: "plan_id", type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ownerstatus = table.Column<string>(name: "owner_status", type: "nvarchar(10)", maxLength: 10, nullable: true),
                    publisheddate = table.Column<DateTime>(name: "published_date", type: "date", nullable: true),
                    publishedplace = table.Column<string>(name: "published_place", type: "nvarchar(50)", maxLength: 50, nullable: true),
                    husbandwifename = table.Column<string>(name: "husband_wife_name", type: "nvarchar(50)", maxLength: 50, nullable: true),
                    representperson = table.Column<string>(name: "represent_person", type: "nvarchar(50)", maxLength: 50, nullable: true),
                    taxpublisheddate = table.Column<DateTime>(name: "tax_published_date", type: "date", nullable: true),
                    organizationtypeid = table.Column<string>(name: "organization_type_id", type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Owners", x => x.ownerid);
                    table.ForeignKey(
                        name: "FK_Owners_OrganizationTypes",
                        column: x => x.organizationtypeid,
                        principalTable: "OrganizationTypes",
                        principalColumn: "organization_type_id");
                    table.ForeignKey(
                        name: "FK_Owners_Plans",
                        column: x => x.planid,
                        principalTable: "Plans",
                        principalColumn: "plan_id");
                    table.ForeignKey(
                        name: "FK_Owners_Projects",
                        column: x => x.projectid,
                        principalTable: "Projects",
                        principalColumn: "project_id");
                });

            migrationBuilder.CreateTable(
                name: "AssetCompensations",
                columns: table => new
                {
                    assetcompensationid = table.Column<string>(name: "asset_compensation_id", type: "nvarchar(50)", maxLength: 50, nullable: false),
                    compensationcontent = table.Column<string>(name: "compensation_content", type: "nvarchar(max)", nullable: true),
                    compensationrate = table.Column<string>(name: "compensation_rate", type: "nvarchar(20)", maxLength: 20, nullable: true),
                    quantityarea = table.Column<int>(name: "quantity_area", type: "int", nullable: true),
                    compensationunit = table.Column<string>(name: "compensation_unit", type: "nvarchar(20)", maxLength: 20, nullable: true),
                    compensationprice = table.Column<decimal>(name: "compensation_price", type: "decimal(10,3)", nullable: true),
                    compensationtype = table.Column<string>(name: "compensation_type", type: "nvarchar(20)", maxLength: 20, nullable: true),
                    unitpriceassetid = table.Column<string>(name: "unit_price_asset_id", type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ownerid = table.Column<string>(name: "owner_id", type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetCompensations", x => x.assetcompensationid);
                    table.ForeignKey(
                        name: "FK_AssetCompensations_Owners",
                        column: x => x.ownerid,
                        principalTable: "Owners",
                        principalColumn: "owner_id");
                    table.ForeignKey(
                        name: "FK_AssetCompensations_UnitPriceAssets",
                        column: x => x.unitpriceassetid,
                        principalTable: "UnitPriceAssets",
                        principalColumn: "unit_price_asset_id");
                });

            migrationBuilder.CreateTable(
                name: "Deductions",
                columns: table => new
                {
                    deductionid = table.Column<string>(name: "deduction_id", type: "nvarchar(50)", maxLength: 50, nullable: false),
                    deductioncontent = table.Column<string>(name: "deduction_content", type: "nvarchar(max)", nullable: true),
                    deductionprice = table.Column<decimal>(name: "deduction_price", type: "decimal(10,3)", nullable: true),
                    ownerid = table.Column<string>(name: "owner_id", type: "nvarchar(50)", maxLength: 50, nullable: true),
                    deductiontypeid = table.Column<string>(name: "deduction_type_id", type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deductions", x => x.deductionid);
                    table.ForeignKey(
                        name: "FK_Deductions_DeductionTypes",
                        column: x => x.deductiontypeid,
                        principalTable: "DeductionTypes",
                        principalColumn: "deduction_type_id");
                    table.ForeignKey(
                        name: "FK_Deductions_Owners",
                        column: x => x.ownerid,
                        principalTable: "Owners",
                        principalColumn: "owner_id");
                });

            migrationBuilder.CreateTable(
                name: "GCNLandInfos",
                columns: table => new
                {
                    GCNlandinfoid = table.Column<string>(name: "GCN_land_info_id", type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GCNpagenumber = table.Column<string>(name: "GCN_page_number", type: "nvarchar(10)", maxLength: 10, nullable: true),
                    GCNplotnumber = table.Column<string>(name: "GCN_plot_number", type: "nvarchar(10)", maxLength: 10, nullable: true),
                    GCNplotaddress = table.Column<string>(name: "GCN_plot_address", type: "nvarchar(100)", maxLength: 100, nullable: true),
                    landtypeid = table.Column<string>(name: "land_type_id", type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GCNplotarea = table.Column<string>(name: "GCN_plot_area", type: "nvarchar(20)", maxLength: 20, nullable: true),
                    GCNownercertificate = table.Column<string>(name: "GCN_owner_certificate", type: "nvarchar(max)", nullable: true),
                    ownerid = table.Column<string>(name: "owner_id", type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GCNLandInfos", x => x.GCNlandinfoid);
                    table.ForeignKey(
                        name: "FK_GCNLandInfos_LandTypes",
                        column: x => x.landtypeid,
                        principalTable: "LandTypes",
                        principalColumn: "land_type_id");
                    table.ForeignKey(
                        name: "FK_GCNLandInfos_Owners",
                        column: x => x.ownerid,
                        principalTable: "Owners",
                        principalColumn: "owner_id");
                });

            migrationBuilder.CreateTable(
                name: "Supports",
                columns: table => new
                {
                    supportid = table.Column<string>(name: "support_id", type: "nvarchar(50)", maxLength: 50, nullable: false),
                    supportcontent = table.Column<string>(name: "support_content", type: "nvarchar(max)", nullable: true),
                    supportunit = table.Column<string>(name: "support_unit", type: "nvarchar(20)", maxLength: 20, nullable: true),
                    supportnumber = table.Column<string>(name: "support_number", type: "nvarchar(10)", maxLength: 10, nullable: true),
                    supportprice = table.Column<decimal>(name: "support_price", type: "decimal(10,3)", nullable: true),
                    ownerid = table.Column<string>(name: "owner_id", type: "nvarchar(50)", maxLength: 50, nullable: true),
                    supporttypeid = table.Column<string>(name: "support_type_id", type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supports", x => x.supportid);
                    table.ForeignKey(
                        name: "FK_Supports_Owners",
                        column: x => x.ownerid,
                        principalTable: "Owners",
                        principalColumn: "owner_id");
                    table.ForeignKey(
                        name: "FK_Supports_SupportTypes",
                        column: x => x.supporttypeid,
                        principalTable: "SupportTypes",
                        principalColumn: "support_type_id");
                });

            migrationBuilder.CreateTable(
                name: "MeasuredLandInfo",
                columns: table => new
                {
                    measuredlandinfoid = table.Column<string>(name: "measured_land_info_id", type: "nvarchar(50)", maxLength: 50, nullable: false),
                    measuredpagenumber = table.Column<string>(name: "measured_page_number", type: "nvarchar(10)", maxLength: 10, nullable: true),
                    measuredplotnumber = table.Column<string>(name: "measured_plot_number", type: "nvarchar(10)", maxLength: 10, nullable: true),
                    measuredplotaddress = table.Column<string>(name: "measured_plot_address", type: "nvarchar(100)", maxLength: 100, nullable: true),
                    landtypeid = table.Column<string>(name: "land_type_id", type: "nvarchar(50)", maxLength: 50, nullable: true),
                    measuredplotarea = table.Column<string>(name: "measured_plot_area", type: "nvarchar(20)", maxLength: 20, nullable: true),
                    widthdrawarea = table.Column<string>(name: "widthdraw_area", type: "nvarchar(20)", maxLength: 20, nullable: true),
                    GCNlandinfoid = table.Column<string>(name: "GCN_land_info_id", type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ownerid = table.Column<string>(name: "owner_id", type: "nvarchar(50)", maxLength: 50, nullable: true),
                    unitpricelandid = table.Column<string>(name: "unit_price_land_id", type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeasuredLandInfo", x => x.measuredlandinfoid);
                    table.ForeignKey(
                        name: "FK_MeasuredLandInfo_GCNLandInfos",
                        column: x => x.GCNlandinfoid,
                        principalTable: "GCNLandInfos",
                        principalColumn: "GCN_land_info_id");
                    table.ForeignKey(
                        name: "FK_MeasuredLandInfo_LandTypes",
                        column: x => x.landtypeid,
                        principalTable: "LandTypes",
                        principalColumn: "land_type_id");
                    table.ForeignKey(
                        name: "FK_MeasuredLandInfo_UnitPriceLands",
                        column: x => x.unitpricelandid,
                        principalTable: "UnitPriceLands",
                        principalColumn: "unit_price_land_id");
                });

            migrationBuilder.CreateTable(
                name: "AttachFiles",
                columns: table => new
                {
                    attachfileid = table.Column<string>(name: "attach_file_id", type: "nvarchar(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    filetype = table.Column<string>(name: "file_type", type: "nvarchar(10)", maxLength: 10, nullable: true),
                    referencelink = table.Column<string>(name: "reference_link", type: "nvarchar(max)", nullable: true),
                    createdtime = table.Column<DateTime>(name: "created_time", type: "datetime", nullable: true),
                    createdby = table.Column<string>(name: "created_by", type: "nvarchar(20)", maxLength: 20, nullable: true),
                    planid = table.Column<string>(name: "plan_id", type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GCNlandinfoid = table.Column<string>(name: "GCN_land_info_id", type: "nvarchar(50)", maxLength: 50, nullable: true),
                    measuredlandinfoid = table.Column<string>(name: "measured_land_info_id", type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ownerid = table.Column<string>(name: "owner_id", type: "nvarchar(50)", maxLength: 50, nullable: true),
                    assetcompensationid = table.Column<string>(name: "asset_compensation_id", type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachFiles", x => x.attachfileid);
                    table.ForeignKey(
                        name: "FK_AttachFiles_AssetCompensations",
                        column: x => x.assetcompensationid,
                        principalTable: "AssetCompensations",
                        principalColumn: "asset_compensation_id");
                    table.ForeignKey(
                        name: "FK_AttachFiles_GCNLandInfos",
                        column: x => x.GCNlandinfoid,
                        principalTable: "GCNLandInfos",
                        principalColumn: "GCN_land_info_id");
                    table.ForeignKey(
                        name: "FK_AttachFiles_MeasuredLandInfo",
                        column: x => x.measuredlandinfoid,
                        principalTable: "MeasuredLandInfo",
                        principalColumn: "measured_land_info_id");
                    table.ForeignKey(
                        name: "FK_AttachFiles_Owners",
                        column: x => x.ownerid,
                        principalTable: "Owners",
                        principalColumn: "owner_id");
                    table.ForeignKey(
                        name: "FK_AttachFiles_Plans",
                        column: x => x.planid,
                        principalTable: "Plans",
                        principalColumn: "plan_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssetCompensations_owner_id",
                table: "AssetCompensations",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_AssetCompensations_unit_price_asset_id",
                table: "AssetCompensations",
                column: "unit_price_asset_id");

            migrationBuilder.CreateIndex(
                name: "IX_AttachFiles_asset_compensation_id",
                table: "AttachFiles",
                column: "asset_compensation_id");

            migrationBuilder.CreateIndex(
                name: "IX_AttachFiles_GCN_land_info_id",
                table: "AttachFiles",
                column: "GCN_land_info_id");

            migrationBuilder.CreateIndex(
                name: "IX_AttachFiles_measured_land_info_id",
                table: "AttachFiles",
                column: "measured_land_info_id");

            migrationBuilder.CreateIndex(
                name: "IX_AttachFiles_owner_id",
                table: "AttachFiles",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_AttachFiles_plan_id",
                table: "AttachFiles",
                column: "plan_id");

            migrationBuilder.CreateIndex(
                name: "IX_Deductions_deduction_type_id",
                table: "Deductions",
                column: "deduction_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_Deductions_owner_id",
                table: "Deductions",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_document_type_id",
                table: "Documents",
                column: "document_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_GCNLandInfos_land_type_id",
                table: "GCNLandInfos",
                column: "land_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_GCNLandInfos_owner_id",
                table: "GCNLandInfos",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_LandPositionInfos_project_id",
                table: "LandPositionInfos",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "IX_LandTypes_land_group_id",
                table: "LandTypes",
                column: "land_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_MeasuredLandInfo_GCN_land_info_id",
                table: "MeasuredLandInfo",
                column: "GCN_land_info_id");

            migrationBuilder.CreateIndex(
                name: "IX_MeasuredLandInfo_land_type_id",
                table: "MeasuredLandInfo",
                column: "land_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_MeasuredLandInfo_unit_price_land_id",
                table: "MeasuredLandInfo",
                column: "unit_price_land_id");

            migrationBuilder.CreateIndex(
                name: "IX_Owners_organization_type_id",
                table: "Owners",
                column: "organization_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_Owners_plan_id",
                table: "Owners",
                column: "plan_id");

            migrationBuilder.CreateIndex(
                name: "IX_Owners_project_id",
                table: "Owners",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_project_id",
                table: "Plans",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDocuments_document_id",
                table: "ProjectDocuments",
                column: "document_id");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDocuments_project_id",
                table: "ProjectDocuments",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_price_applied_code_id",
                table: "Projects",
                column: "price_applied_code_id");

            migrationBuilder.CreateIndex(
                name: "IX_Supports_owner_id",
                table: "Supports",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_Supports_support_type_id",
                table: "Supports",
                column: "support_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_UnitPriceAssets_asset_group_id",
                table: "UnitPriceAssets",
                column: "asset_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_UnitPriceAssets_asset_unit_id",
                table: "UnitPriceAssets",
                column: "asset_unit_id");

            migrationBuilder.CreateIndex(
                name: "IX_UnitPriceAssets_price_applied_code_id",
                table: "UnitPriceAssets",
                column: "price_applied_code_id");

            migrationBuilder.CreateIndex(
                name: "IX_UnitPriceLands_land_type_id",
                table: "UnitPriceLands",
                column: "land_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_UnitPriceLands_project_id",
                table: "UnitPriceLands",
                column: "project_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttachFiles");

            migrationBuilder.DropTable(
                name: "Deductions");

            migrationBuilder.DropTable(
                name: "LandPositionInfos");

            migrationBuilder.DropTable(
                name: "ProjectDocuments");

            migrationBuilder.DropTable(
                name: "Supports");

            migrationBuilder.DropTable(
                name: "AssetCompensations");

            migrationBuilder.DropTable(
                name: "MeasuredLandInfo");

            migrationBuilder.DropTable(
                name: "DeductionTypes");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "SupportTypes");

            migrationBuilder.DropTable(
                name: "UnitPriceAssets");

            migrationBuilder.DropTable(
                name: "GCNLandInfos");

            migrationBuilder.DropTable(
                name: "UnitPriceLands");

            migrationBuilder.DropTable(
                name: "DocumentTypes");

            migrationBuilder.DropTable(
                name: "AssetGroups");

            migrationBuilder.DropTable(
                name: "AssetUnits");

            migrationBuilder.DropTable(
                name: "Owners");

            migrationBuilder.DropTable(
                name: "LandTypes");

            migrationBuilder.DropTable(
                name: "OrganizationTypes");

            migrationBuilder.DropTable(
                name: "Plans");

            migrationBuilder.DropTable(
                name: "LandGroups");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "PriceAppliedCodes");
        }
    }
}
