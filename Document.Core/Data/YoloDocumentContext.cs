using Document.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Document.Core.Data;

public partial class YoloDocumentContext : DbContext
{
    public YoloDocumentContext()
    {
    }

    public YoloDocumentContext(DbContextOptions<YoloDocumentContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AuditTrail> AuditTrails { get; set; }

    public virtual DbSet<Entities.Document> Documents { get; set; }

    public virtual DbSet<FileVersion> FileVersions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditTrail>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.AffectedColumn).HasColumnName("affected_column");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.NewValue).HasColumnName("new_value");
            entity.Property(e => e.OldValue).HasColumnName("old_value");
            entity.Property(e => e.PrimaryKey).HasColumnName("primary_key");
            entity.Property(e => e.TableName).HasColumnName("table_name");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.UserName).HasColumnName("user_name");
        });

        modelBuilder.Entity<Entities.Document>(entity =>
        {
            entity.Property(e => e.DocumentId)
                .HasMaxLength(50)
                .HasColumnName("document_id");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .HasColumnName("created_by");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");
        });

        modelBuilder.Entity<FileVersion>(entity =>
        {
            entity.HasKey(e => e.VersionId);

            entity.Property(e => e.VersionId)
                .HasMaxLength(50)
                .HasColumnName("version_id");
            entity.Property(e => e.ChangeDescription).HasColumnName("change_description");
            entity.Property(e => e.CreatedTime)
                .HasColumnType("datetime")
                .HasColumnName("created_time");
            entity.Property(e => e.DocumentId)
                .HasMaxLength(50)
                .HasColumnName("document_id");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.IsReady).HasColumnName("is_ready");
            entity.Property(e => e.ReferenceLink).HasColumnName("reference_link");
            entity.Property(e => e.VersionNumber).HasColumnName("version_number");

            entity.HasOne(d => d.Document).WithMany(p => p.FileVersions)
                .HasForeignKey(d => d.DocumentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FileVersions_Documents");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
