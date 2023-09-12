using System;
using System.Collections.Generic;
using Auth.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Auth.Core.Data;

public partial class YoloAuthContext : DbContext
{
    public YoloAuthContext()
    {
    }

    public YoloAuthContext(DbContextOptions<YoloAuthContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Role> Roles { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.Property(e => e.Id).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(30);
            entity.Property(e => e.Name).HasMaxLength(30);
            entity.Property(e => e.Password).HasMaxLength(20);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.RoleId).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(20);

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Accounts_Roles");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.Id).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(10);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
