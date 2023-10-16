
using Metadata.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Metadata.Core.Data;

public partial class YoloMetadataContext : DbContext
{
    public YoloMetadataContext()
    {
    }

    public YoloMetadataContext(DbContextOptions<YoloMetadataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AssetCompensation> AssetCompensations { get; set; }

    public virtual DbSet<AssetGroup> AssetGroups { get; set; }

    public virtual DbSet<AssetUnit> AssetUnits { get; set; }

    public virtual DbSet<AttachFile> AttachFiles { get; set; }

    public virtual DbSet<AuditTrail> AuditTrails { get; set; }

    public virtual DbSet<Deduction> Deductions { get; set; }

    public virtual DbSet<DeductionType> DeductionTypes { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<DocumentType> DocumentTypes { get; set; }

    public virtual DbSet<GcnlandInfo> GcnlandInfos { get; set; }

    public virtual DbSet<LandGroup> LandGroups { get; set; }

    public virtual DbSet<LandPositionInfo> LandPositionInfos { get; set; }

    public virtual DbSet<LandType> LandTypes { get; set; }

    public virtual DbSet<MeasuredLandInfo> MeasuredLandInfos { get; set; }

    public virtual DbSet<OrganizationType> OrganizationTypes { get; set; }

    public virtual DbSet<Owner> Owners { get; set; }

    public virtual DbSet<Plan> Plans { get; set; }

    public virtual DbSet<PriceAppliedCode> PriceAppliedCodes { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<ProjectDocument> ProjectDocuments { get; set; }

    public virtual DbSet<Support> Supports { get; set; }

    public virtual DbSet<SupportType> SupportTypes { get; set; }

    public virtual DbSet<UnitPriceAsset> UnitPriceAssets { get; set; }

    public virtual DbSet<UnitPriceLand> UnitPriceLands { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AssetCompensation>(entity =>
        {
            entity.Property(e => e.AssetCompensationId)
                .HasMaxLength(50)
                .HasColumnName("asset_compensation_id");
            entity.Property(e => e.CompensationContent).HasColumnName("compensation_content");
            entity.Property(e => e.CompensationPrice)
                .HasColumnType("decimal(10, 3)")
                .HasColumnName("compensation_price");
            entity.Property(e => e.CompensationRate)
                .HasMaxLength(20)
                .HasColumnName("compensation_rate");
            entity.Property(e => e.CompensationType)
                .HasMaxLength(20)
                .HasColumnName("compensation_type");
            entity.Property(e => e.CompensationUnit)
                .HasMaxLength(20)
                .HasColumnName("compensation_unit");
            entity.Property(e => e.OwnerId)
                .HasMaxLength(50)
                .HasColumnName("owner_id");
            entity.Property(e => e.QuantityArea).HasColumnName("quantity_area");
            entity.Property(e => e.UnitPriceAssetId)
                .HasMaxLength(50)
                .HasColumnName("unit_price_asset_id");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

            entity.HasOne(d => d.Owner).WithMany(p => p.AssetCompensations)
                .HasForeignKey(d => d.OwnerId)
                .HasConstraintName("FK_AssetCompensations_Owners");

            entity.HasOne(d => d.UnitPriceAsset).WithMany(p => p.AssetCompensations)
                .HasForeignKey(d => d.UnitPriceAssetId)
                .HasConstraintName("FK_AssetCompensations_UnitPriceAssets");
        });

        modelBuilder.Entity<AssetGroup>(entity =>
        {
            entity.Property(e => e.AssetGroupId)
                .HasMaxLength(50)
                .HasColumnName("asset_group_id");
            entity.Property(e => e.Code)
                .HasMaxLength(10)
                .HasColumnName("code");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .HasColumnName("name");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        });

        modelBuilder.Entity<AssetUnit>(entity =>
        {
            entity.Property(e => e.AssetUnitId)
                .HasMaxLength(50)
                .HasColumnName("asset_unit_id");
            entity.Property(e => e.Code)
                .HasMaxLength(10)
                .HasColumnName("code");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .HasColumnName("name");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        });

        modelBuilder.Entity<AttachFile>(entity =>
        {
            entity.Property(e => e.AttachFileId)
                .HasMaxLength(50)
                .HasColumnName("attach_file_id");
            entity.Property(e => e.AssetCompensationId)
                .HasMaxLength(50)
                .HasColumnName("asset_compensation_id");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(20)
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedTime)
                .HasColumnType("datetime")
                .HasColumnName("created_time");
            entity.Property(e => e.FileType)
                .HasMaxLength(10)
                .HasColumnName("file_type");
            entity.Property(e => e.GcnLandInfoId)
                .HasMaxLength(50)
                .HasColumnName("GCN_land_info_id");
            entity.Property(e => e.MeasuredLandInfoId)
                .HasMaxLength(50)
                .HasColumnName("measured_land_info_id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.OwnerId)
                .HasMaxLength(50)
                .HasColumnName("owner_id");
            entity.Property(e => e.PlanId)
                .HasMaxLength(50)
                .HasColumnName("plan_id");
            entity.Property(e => e.ReferenceLink).HasColumnName("reference_link");

            entity.HasOne(d => d.AssetCompensation).WithMany(p => p.AttachFiles)
                .HasForeignKey(d => d.AssetCompensationId)
                .HasConstraintName("FK_AttachFiles_AssetCompensations");

            entity.HasOne(d => d.GcnLandInfo).WithMany(p => p.AttachFiles)
                .HasForeignKey(d => d.GcnLandInfoId)
                .HasConstraintName("FK_AttachFiles_GCNLandInfos");

            entity.HasOne(d => d.MeasuredLandInfo).WithMany(p => p.AttachFiles)
                .HasForeignKey(d => d.MeasuredLandInfoId)
                .HasConstraintName("FK_AttachFiles_MeasuredLandInfo");

            entity.HasOne(d => d.Owner).WithMany(p => p.AttachFiles)
                .HasForeignKey(d => d.OwnerId)
                .HasConstraintName("FK_AttachFiles_Owners");

            entity.HasOne(d => d.Plan).WithMany(p => p.AttachFiles)
                .HasForeignKey(d => d.PlanId)
                .HasConstraintName("FK_AttachFiles_Plans");
        });


        modelBuilder.Entity<AuditTrail>(entity =>
        {
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.UserId)
                .HasColumnName("user_id");
            entity.Property(e => e.UserName)
                .HasColumnName("user_name");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.Type)
                .HasColumnName("type");
            entity.Property(e => e.TableName)
                .HasColumnName("table_name");
            entity.Property(e => e.NewValue).HasColumnName("new_value");
            entity.Property(e => e.OldValue).HasColumnName("old_value");
            entity.Property(e => e.AffectedColumn).HasColumnName("affected_column");
            entity.Property(e => e.PrimaryKey).HasColumnName("primary_key");
        });

        modelBuilder.Entity<Deduction>(entity =>
        {
            entity.Property(e => e.DeductionId)
                .HasMaxLength(50)
                .HasColumnName("deduction_id");
            entity.Property(e => e.DeductionContent).HasColumnName("deduction_content");
            entity.Property(e => e.DeductionPrice)
                .HasColumnType("decimal(10, 3)")
                .HasColumnName("deduction_price");
            entity.Property(e => e.DeductionTypeId)
                .HasMaxLength(50)
                .HasColumnName("deduction_type_id");
            entity.Property(e => e.OwnerId)
                .HasMaxLength(50)
                .HasColumnName("owner_id");

            entity.HasOne(d => d.DeductionType).WithMany(p => p.Deductions)
                .HasForeignKey(d => d.DeductionTypeId)
                .HasConstraintName("FK_Deductions_DeductionTypes");

            entity.HasOne(d => d.Owner).WithMany(p => p.Deductions)
                .HasForeignKey(d => d.OwnerId)
                .HasConstraintName("FK_Deductions_Owners");
        });

        modelBuilder.Entity<DeductionType>(entity =>
        {
            entity.Property(e => e.DeductionTypeId)
                .HasMaxLength(50)
                .HasColumnName("deduction_type_id");
            entity.Property(e => e.Code)
                .HasMaxLength(10)
                .HasColumnName("code");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.Property(e => e.DocumentId)
                .HasMaxLength(50)
                .HasColumnName("document_id");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(20)
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedTime)
                .HasColumnType("datetime")
                .HasColumnName("created_time");
            entity.Property(e => e.DocumentTypeId)
                .HasMaxLength(50)
                .HasColumnName("document_type_id");
            entity.Property(e => e.EffectiveDate)
                .HasColumnType("date")
                .HasColumnName("effective_date");
            entity.Property(e => e.Epitome).HasColumnName("epitome");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.IsPublic).HasColumnName("is_public");
            entity.Property(e => e.Notation)
                .HasMaxLength(10)
                .HasColumnName("notation");
            entity.Property(e => e.Note).HasColumnName("note");
            entity.Property(e => e.Number)
                .HasMaxLength(10)
                .HasColumnName("number");
            entity.Property(e => e.Pen).HasColumnName("pen");
            entity.Property(e => e.PublishedDate)
                .HasColumnType("date")
                .HasColumnName("published_date");
            entity.Property(e => e.ReferenceLink).HasColumnName("reference_link");
            entity.Property(e => e.SignInfo).HasColumnName("sign_info");

            entity.HasOne(d => d.DocumentType).WithMany(p => p.Documents)
                .HasForeignKey(d => d.DocumentTypeId)
                .HasConstraintName("FK_Documents_DocumentTypes");
        });

        modelBuilder.Entity<DocumentType>(entity =>
        {
            entity.Property(e => e.DocumentTypeId)
                .HasMaxLength(50)
                .HasColumnName("document_type_id");
            entity.Property(e => e.Code)
                .HasMaxLength(6)
                .HasColumnName("code");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<GcnlandInfo>(entity =>
        {
            entity.ToTable("GCNLandInfos");

            entity.Property(e => e.GcnLandInfoId)
                .HasMaxLength(50)
                .HasColumnName("GCN_land_info_id");
            entity.Property(e => e.GcnOwnerCertificate).HasColumnName("GCN_owner_certificate");
            entity.Property(e => e.GcnPageNumber)
                .HasMaxLength(10)
                .HasColumnName("GCN_page_number");
            entity.Property(e => e.GcnPlotAddress)
                .HasMaxLength(100)
                .HasColumnName("GCN_plot_address");
            entity.Property(e => e.GcnPlotArea)
                .HasMaxLength(20)
                .HasColumnName("GCN_plot_area");
            entity.Property(e => e.GcnPlotNumber)
                .HasMaxLength(10)
                .HasColumnName("GCN_plot_number");
            entity.Property(e => e.LandTypeId)
                .HasMaxLength(50)
                .HasColumnName("land_type_id");
            entity.Property(e => e.OwnerId)
                .HasMaxLength(50)
                .HasColumnName("owner_id");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.HasOne(d => d.LandType).WithMany(p => p.GcnlandInfos)
                .HasForeignKey(d => d.LandTypeId)
                .HasConstraintName("FK_GCNLandInfos_LandTypes");

            entity.HasOne(d => d.Owner).WithMany(p => p.GcnlandInfos)
                .HasForeignKey(d => d.OwnerId)
                .HasConstraintName("FK_GCNLandInfos_Owners");
        });

        modelBuilder.Entity<LandGroup>(entity =>
        {
            entity.Property(e => e.LandGroupId)
                .HasMaxLength(50)
                .HasColumnName("land_group_id");
            entity.Property(e => e.Code)
                .HasMaxLength(10)
                .HasColumnName("code");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .HasColumnName("name");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        });

        modelBuilder.Entity<LandPositionInfo>(entity =>
        {
            entity.HasKey(e => e.LandInfoPositionId);

            entity.Property(e => e.LandInfoPositionId)
                .HasMaxLength(50)
                .HasColumnName("land_info_position_id");
            entity.Property(e => e.Description)
                .HasColumnType("ntext")
                .HasColumnName("description");
            entity.Property(e => e.LocationName)
                .HasMaxLength(50)
                .HasColumnName("location_name");
            entity.Property(e => e.ProjectId)
                .HasMaxLength(50)
                .HasColumnName("project_id");

            entity.HasOne(d => d.Project).WithMany(p => p.LandPositionInfos)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK_LandPositionInfos_Projects");
        });

        modelBuilder.Entity<LandType>(entity =>
        {
            entity.Property(e => e.LandTypeId)
                .HasMaxLength(50)
                .HasColumnName("land_type_id");
            entity.Property(e => e.Code)
                .HasMaxLength(10)
                .HasColumnName("code");
            entity.Property(e => e.LandGroupId)
                .HasMaxLength(50)
                .HasColumnName("land_group_id");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .HasColumnName("name");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.HasOne(d => d.LandGroup).WithMany(p => p.LandTypes)
                .HasForeignKey(d => d.LandGroupId)
                .HasConstraintName("FK_LandTypes_LandGroups");
        });

        modelBuilder.Entity<MeasuredLandInfo>(entity =>
        {
            entity.ToTable("MeasuredLandInfo");

            entity.Property(e => e.MeasuredLandInfoId)
                .HasMaxLength(50)
                .HasColumnName("measured_land_info_id");
            entity.Property(e => e.GcnLandInfoId)
                .HasMaxLength(50)
                .HasColumnName("GCN_land_info_id");
            entity.Property(e => e.LandTypeId)
                .HasMaxLength(50)
                .HasColumnName("land_type_id");
            entity.Property(e => e.MeasuredPageNumber)
                .HasMaxLength(10)
                .HasColumnName("measured_page_number");
            entity.Property(e => e.MeasuredPlotAddress)
                .HasMaxLength(100)
                .HasColumnName("measured_plot_address");
            entity.Property(e => e.MeasuredPlotArea)
                .HasMaxLength(20)
                .HasColumnName("measured_plot_area");
            entity.Property(e => e.MeasuredPlotNumber)
                .HasMaxLength(10)
                .HasColumnName("measured_plot_number");
            entity.Property(e => e.OwnerId)
                .HasMaxLength(50)
                .HasColumnName("owner_id");
            entity.Property(e => e.UnitPriceLandId)
                .HasMaxLength(50)
                .HasColumnName("unit_price_land_id");
            entity.Property(e => e.WidthdrawArea)
                .HasMaxLength(20)
                .HasColumnName("widthdraw_area");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.HasOne(d => d.GcnLandInfo).WithMany(p => p.MeasuredLandInfos)
                .HasForeignKey(d => d.GcnLandInfoId)
                .HasConstraintName("FK_MeasuredLandInfo_GCNLandInfos");

            entity.HasOne(d => d.LandType).WithMany(p => p.MeasuredLandInfos)
                .HasForeignKey(d => d.LandTypeId)
                .HasConstraintName("FK_MeasuredLandInfo_LandTypes");

            entity.HasOne(d => d.UnitPriceLand).WithMany(p => p.MeasuredLandInfos)
                .HasForeignKey(d => d.UnitPriceLandId)
                .HasConstraintName("FK_MeasuredLandInfo_UnitPriceLands");
        });

        modelBuilder.Entity<OrganizationType>(entity =>
        {
            entity.Property(e => e.OrganizationTypeId)
                .HasMaxLength(50)
                .HasColumnName("organization_type_id");
            entity.Property(e => e.Code)
                .HasMaxLength(20)
                .HasColumnName("code");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .HasColumnName("name");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        });

        modelBuilder.Entity<Owner>(entity =>
        {
            entity.Property(e => e.OwnerId)
                .HasMaxLength(50)
                .HasColumnName("owner_id");
            entity.Property(e => e.HusbandWifeName)
                .HasMaxLength(50)
                .HasColumnName("husband_wife_name");
            entity.Property(e => e.OrganizationTypeId)
                .HasMaxLength(50)
                .HasColumnName("organization_type_id");
            entity.Property(e => e.OwnerAddress)
                .HasMaxLength(200)
                .HasColumnName("owner_address");
            entity.Property(e => e.OwnerCode)
                .HasMaxLength(20)
                .HasColumnName("owner_code");
            entity.Property(e => e.OwnerCreatedBy)
                .HasMaxLength(20)
                .HasColumnName("owner_created_by");
            entity.Property(e => e.OwnerCreatedTime)
                .HasColumnType("datetime")
                .HasColumnName("owner_created_time");
            entity.Property(e => e.OwnerDateOfBirth)
                .HasColumnType("date")
                .HasColumnName("owner_date_of_birth");
            entity.Property(e => e.OwnerEthnic)
                .HasMaxLength(50)
                .HasColumnName("owner_ethnic");
            entity.Property(e => e.OwnerGender)
                .HasMaxLength(10)
                .HasColumnName("owner_gender");
            entity.Property(e => e.OwnerIdCode)
                .HasMaxLength(20)
                .HasColumnName("owner_id_code");
            entity.Property(e => e.OwnerName)
                .HasMaxLength(50)
                .HasColumnName("owner_name");
            entity.Property(e => e.OwnerNational)
                .HasMaxLength(50)
                .HasColumnName("owner_national");
            entity.Property(e => e.OwnerStatus)
                .HasMaxLength(10)
                .HasColumnName("owner_status");
            entity.Property(e => e.OwnerTaxCode)
                .HasMaxLength(10)
                .HasColumnName("owner_tax_code");
            entity.Property(e => e.OwnerType)
                .HasMaxLength(20)
                .HasColumnName("owner_type");
            entity.Property(e => e.PlanId)
                .HasMaxLength(50)
                .HasColumnName("plan_id");
            entity.Property(e => e.ProjectId)
                .HasMaxLength(50)
                .HasColumnName("project_id");
            entity.Property(e => e.PublishedDate)
                .HasColumnType("date")
                .HasColumnName("published_date");
            entity.Property(e => e.PublishedPlace)
                .HasMaxLength(50)
                .HasColumnName("published_place");
            entity.Property(e => e.RepresentPerson)
                .HasMaxLength(50)
                .HasColumnName("represent_person");
            entity.Property(e => e.TaxPublishedDate)
                .HasColumnType("date")
                .HasColumnName("tax_published_date");

            entity.HasOne(d => d.OrganizationType).WithMany(p => p.Owners)
                .HasForeignKey(d => d.OrganizationTypeId)
                .HasConstraintName("FK_Owners_OrganizationTypes");

            entity.HasOne(d => d.Plan).WithMany(p => p.Owners)
                .HasForeignKey(d => d.PlanId)
                .HasConstraintName("FK_Owners_Plans");

            entity.HasOne(d => d.Project).WithMany(p => p.Owners)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK_Owners_Projects");
        });

        modelBuilder.Entity<Plan>(entity =>
        {
            entity.Property(e => e.PlanId)
                .HasMaxLength(50)
                .HasColumnName("plan_id");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.PlanApprovedBy)
                .HasMaxLength(50)
                .HasColumnName("plan_approved_by");
            entity.Property(e => e.PlanCreateBase)
                .HasMaxLength(50)
                .HasColumnName("plan_create_base");
            entity.Property(e => e.PlanCreatedBy)
                .HasMaxLength(20)
                .HasColumnName("plan_created_by");
            entity.Property(e => e.PlanCreatedTime)
                .HasColumnType("datetime")
                .HasColumnName("plan_created_time");
            entity.Property(e => e.PlanDescription)
                .HasColumnType("ntext")
                .HasColumnName("plan_description");
            entity.Property(e => e.PlanEndedTime)
                .HasColumnType("datetime")
                .HasColumnName("plan_ended_time");
            entity.Property(e => e.PlanPhrase)
                .HasMaxLength(10)
                .HasColumnName("plan_phrase");
            entity.Property(e => e.PlanReportDate)
                .HasColumnType("date")
                .HasColumnName("plan_report_date");
            entity.Property(e => e.PlanReportSignal)
                .HasColumnType("ntext")
                .HasColumnName("plan_report_signal");
            entity.Property(e => e.PlanStatus).HasColumnName("plan_status");
            entity.Property(e => e.PlaneCode)
                .HasMaxLength(10)
                .HasColumnName("plane_code");
            entity.Property(e => e.ProjectId)
                .HasMaxLength(50)
                .HasColumnName("project_id");

            entity.HasOne(d => d.Project).WithMany(p => p.Plans)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK_Plans_Projects");
        });

        modelBuilder.Entity<PriceAppliedCode>(entity =>
        {
            entity.HasKey(e => e.PriceAppliedCodeId).HasName("PK_PriceAppliedCode");

            entity.Property(e => e.PriceAppliedCodeId)
                .HasMaxLength(50)
                .HasColumnName("price_applied_code_id");
            entity.Property(e => e.ExpriredTime)
                .HasColumnType("datetime")
                .HasColumnName("exprired_time");
            entity.Property(e => e.PriceContent)
                .HasColumnType("ntext")
                .HasColumnName("price_content");
            entity.Property(e => e.UnitPriceCode)
                .HasMaxLength(20)
                .HasColumnName("unit_price_code");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.ProjectId).HasName("PK_Project");

            entity.Property(e => e.ProjectId)
                .HasMaxLength(50)
                .HasColumnName("project_id");
            entity.Property(e => e.AssetCompensationBasis)
                .HasMaxLength(20)
                .HasColumnName("asset_compensation_basis");
            entity.Property(e => e.CheckCode)
                .HasMaxLength(20)
                .HasColumnName("check_code");
            entity.Property(e => e.District)
                .HasMaxLength(20)
                .HasColumnName("district");
            entity.Property(e => e.ImplementationYear)
                .HasMaxLength(4)
                .HasColumnName("implementation_year");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.LandCompensationBasis)
                .HasMaxLength(20)
                .HasColumnName("land_compensation_basis");
            entity.Property(e => e.PriceAppliedCodeId)
                .HasMaxLength(50)
                .HasColumnName("price_applied_code_id");
            entity.Property(e => e.PriceBasis)
                .HasMaxLength(20)
                .HasColumnName("price_basis");
            entity.Property(e => e.ProjectApprovalDate)
                .HasColumnType("date")
                .HasColumnName("project_approval_date");
            entity.Property(e => e.ProjectBriefNumber)
                .HasMaxLength(10)
                .HasColumnName("project_brief_number");
            entity.Property(e => e.ProjectCode)
                .HasMaxLength(50)
                .HasColumnName("project_code");
            entity.Property(e => e.ProjectCreatedBy)
                .HasMaxLength(20)
                .HasColumnName("project_created_by");
            entity.Property(e => e.ProjectCreatedTime)
                .HasColumnType("date")
                .HasColumnName("project_created_time");
            entity.Property(e => e.ProjectExpense)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("project_expense");
            entity.Property(e => e.ProjectLocation)
                .HasMaxLength(200)
                .HasColumnName("project_location");
            entity.Property(e => e.ProjectName)
                .HasMaxLength(50)
                .HasColumnName("project_name");
            entity.Property(e => e.ProjectNote)
                .HasMaxLength(50)
                .HasColumnName("project_note");
            entity.Property(e => e.ProjectStatus).HasColumnName("project_status");
            entity.Property(e => e.Province)
                .HasMaxLength(20)
                .HasColumnName("province");
            entity.Property(e => e.RegulatedUnitPrice)
                .HasMaxLength(20)
                .HasColumnName("regulated_unit_price");
            entity.Property(e => e.ReportNumber)
                .HasMaxLength(20)
                .HasColumnName("report_number");
            entity.Property(e => e.ReportSignal)
                .HasColumnType("ntext")
                .HasColumnName("report_signal");
            entity.Property(e => e.Ward)
                .HasMaxLength(20)
                .HasColumnName("ward");

            entity.HasOne(d => d.PriceAppliedCode).WithMany(p => p.Projects)
                .HasForeignKey(d => d.PriceAppliedCodeId)
                .HasConstraintName("FK_Projects_PriceAppliedCodes");
        });

        modelBuilder.Entity<ProjectDocument>(entity =>
        {
            entity.Property(e => e.ProjectDocumentId)
                .HasMaxLength(50)
                .HasColumnName("project_document_id");
            entity.Property(e => e.DocumentId)
                .HasMaxLength(50)
                .HasColumnName("document_id");
            entity.Property(e => e.ProjectId)
                .HasMaxLength(50)
                .HasColumnName("project_id");

            entity.HasOne(d => d.Document).WithMany(p => p.ProjectDocuments)
                .HasForeignKey(d => d.DocumentId)
                .HasConstraintName("FK_ProjectDocuments_Documents");

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectDocuments)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK_ProjectDocuments_Projects");
        });

        modelBuilder.Entity<Support>(entity =>
        {
            entity.Property(e => e.SupportId)
                .HasMaxLength(50)
                .HasColumnName("support_id");
            entity.Property(e => e.OwnerId)
                .HasMaxLength(50)
                .HasColumnName("owner_id");
            entity.Property(e => e.SupportContent).HasColumnName("support_content");
            entity.Property(e => e.SupportNumber)
                .HasMaxLength(10)
                .HasColumnName("support_number");
            entity.Property(e => e.SupportPrice)
                .HasColumnType("decimal(10, 3)")
                .HasColumnName("support_price");
            entity.Property(e => e.SupportTypeId)
                .HasMaxLength(50)
                .HasColumnName("support_type_id");
            entity.Property(e => e.SupportUnit)
                .HasMaxLength(20)
                .HasColumnName("support_unit");

            entity.HasOne(d => d.Owner).WithMany(p => p.Supports)
                .HasForeignKey(d => d.OwnerId)
                .HasConstraintName("FK_Supports_Owners");

            entity.HasOne(d => d.SupportType).WithMany(p => p.Supports)
                .HasForeignKey(d => d.SupportTypeId)
                .HasConstraintName("FK_Supports_SupportTypes");
        });

        modelBuilder.Entity<SupportType>(entity =>
        {
            entity.HasKey(e => e.SupportTypeId).HasName("PK_SupportType");

            entity.Property(e => e.SupportTypeId)
                .HasMaxLength(50)
                .HasColumnName("support_type_id");
            entity.Property(e => e.Code)
                .HasMaxLength(10)
                .HasColumnName("code");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        });

        modelBuilder.Entity<UnitPriceAsset>(entity =>
        {
            entity.Property(e => e.UnitPriceAssetId)
                .HasMaxLength(50)
                .HasColumnName("unit_price_asset_id");
            entity.Property(e => e.AssetGroupId)
                .HasMaxLength(50)
                .HasColumnName("asset_group_id");
            entity.Property(e => e.AssetName)
                .HasMaxLength(20)
                .HasColumnName("asset_name");
            entity.Property(e => e.AssetPrice)
                .HasColumnType("decimal(10, 3)")
                .HasColumnName("asset_price");
            entity.Property(e => e.AssetRegulation)
                .HasMaxLength(20)
                .HasColumnName("asset_regulation");
            entity.Property(e => e.AssetType)
                .HasMaxLength(20)
                .HasColumnName("asset_type");
            entity.Property(e => e.AssetUnitId)
                .HasMaxLength(50)
                .HasColumnName("asset_unit_id");
            entity.Property(e => e.PriceAppliedCodeId)
                .HasMaxLength(50)
                .HasColumnName("price_applied_code_id");

            entity.HasOne(d => d.AssetGroup).WithMany(p => p.UnitPriceAssets)
                .HasForeignKey(d => d.AssetGroupId)
                .HasConstraintName("FK_UnitPriceAssets_AssetGroups");

            entity.HasOne(d => d.AssetUnit).WithMany(p => p.UnitPriceAssets)
                .HasForeignKey(d => d.AssetUnitId)
                .HasConstraintName("FK_UnitPriceAssets_AssetUnits");

            entity.HasOne(d => d.PriceAppliedCode).WithMany(p => p.UnitPriceAssets)
                .HasForeignKey(d => d.PriceAppliedCodeId)
                .HasConstraintName("FK_UnitPriceAssets_PriceAppliedCodes");
        });

        modelBuilder.Entity<UnitPriceLand>(entity =>
        {
            entity.Property(e => e.UnitPriceLandId)
                .HasMaxLength(50)
                .HasColumnName("unit_price_land_id");
            entity.Property(e => e.LandPosition1)
                .HasColumnType("decimal(10, 3)")
                .HasColumnName("land_position_1");
            entity.Property(e => e.LandPosition2)
                .HasColumnType("decimal(10, 3)")
                .HasColumnName("land_position_2");
            entity.Property(e => e.LandPosition3)
                .HasColumnType("decimal(10, 3)")
                .HasColumnName("land_position_3");
            entity.Property(e => e.LandPosition4)
                .HasColumnType("decimal(10, 3)")
                .HasColumnName("land_position_4");
            entity.Property(e => e.LandPositionRest)
                .HasColumnType("decimal(10, 3)")
                .HasColumnName("land_position_rest");
            entity.Property(e => e.LandTypeId)
                .HasMaxLength(50)
                .HasColumnName("land_type_id");
            entity.Property(e => e.LandUnit)
                .HasMaxLength(20)
                .HasColumnName("land_unit");
            entity.Property(e => e.ProjectId)
                .HasMaxLength(50)
                .HasColumnName("project_id");
            entity.Property(e => e.StreetAreaName)
                .HasMaxLength(50)
                .HasColumnName("street_area_name");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.HasOne(d => d.LandType).WithMany(p => p.UnitPriceLands)
                .HasForeignKey(d => d.LandTypeId)
                .HasConstraintName("FK_UnitPriceLands_LandTypes");

            entity.HasOne(d => d.Project).WithMany(p => p.UnitPriceLands)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK_UnitPriceLands_Projects");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
