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
    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

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
            entity.Property(e=>e.Otp).HasMaxLength(6);

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Accounts_Roles");
        });


        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_RefreshToken");

            entity.Property(e => e.Id).HasMaxLength(50);
            entity.Property(e => e.AccountId).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Expires).HasColumnType("datetime");
            entity.Property(e => e.ReplacedBy).HasMaxLength(50);
            entity.Property(e => e.Token).HasMaxLength(150);

            entity.HasOne(d => d.Account).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_RefreshTokens_Accounts");
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
