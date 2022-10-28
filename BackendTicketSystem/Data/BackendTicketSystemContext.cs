using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using BackendTicketSystem.Models;

namespace BackendTicketSystem.Data
{
    public partial class BackendTicketSystemContext : DbContext
    {
        public BackendTicketSystemContext()
        {
        }

        public BackendTicketSystemContext(DbContextOptions<BackendTicketSystemContext> options)
            : base(options)
        {
        }

        public virtual DbSet<GlobalParam> GlobalParams { get; set; } = null!;
        public virtual DbSet<Project> Projects { get; set; } = null!;
        public virtual DbSet<ProjectPackage> ProjectPackages { get; set; } = null!;
        public virtual DbSet<ProjectType> ProjectTypes { get; set; } = null!;
        public virtual DbSet<Status> Statuses { get; set; } = null!;
        public virtual DbSet<Ticket> Tickets { get; set; } = null!;
        public virtual DbSet<TicketAction> TicketActions { get; set; } = null!;
        public virtual DbSet<TicketActionFile> TicketActionFiles { get; set; } = null!;
        public virtual DbSet<TicketType> TicketTypes { get; set; } = null!;
        public virtual DbSet<UserAccount> UserAccounts { get; set; } = null!;
        public virtual DbSet<UserAccountToken> UserAccountTokens { get; set; } = null!;
        public virtual DbSet<UserRole> UserRoles { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=localhost;Database=BackendTicketSystem; Integrated Security=false;User ID=sa;Password=MyPass@word;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GlobalParam>(entity =>
            {
                entity.ToTable("GlobalParam");

                entity.Property(e => e.KeyName)
                    .HasMaxLength(100)
                    .UseCollation("SQL_Latin1_General_CP850_BIN");

                entity.Property(e => e.Memo)
                    .HasMaxLength(500)
                    .UseCollation("SQL_Latin1_General_CP850_BIN");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .UseCollation("SQL_Latin1_General_CP850_BIN");

                entity.Property(e => e.Type)
                    .HasMaxLength(100)
                    .UseCollation("SQL_Latin1_General_CP850_BIN");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.GlobalParams)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GlobalParam_Statuses");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("Project");

                entity.Property(e => e.ApplicationName).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DatabaseName)
                    .HasMaxLength(100)
                    .UseCollation("SQL_Latin1_General_CP850_BIN");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .UseCollation("SQL_Latin1_General_CP850_BIN");

                entity.Property(e => e.StatusId).HasDefaultValueSql("((1))");

                entity.Property(e => e.Version).HasDefaultValueSql("((1))");

                entity.Property(e => e.WebsiteUrl)
                    .HasMaxLength(100)
                    .HasColumnName("WebsiteURL")
                    .UseCollation("SQL_Latin1_General_CP850_BIN");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.ProjectCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Project_CreatedBy_UserAccount");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.ProjectModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_Project_ModifiedBy_UserAccount");

                entity.HasOne(d => d.ProjectPackage)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.ProjectPackageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Project_ProjectPackage");

                entity.HasOne(d => d.ProjectType)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.ProjectTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Project_ProjectType");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Project_Statuses");
            });

            modelBuilder.Entity<ProjectPackage>(entity =>
            {
                entity.ToTable("ProjectPackage");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Memo)
                    .HasMaxLength(500)
                    .UseCollation("SQL_Latin1_General_CP850_BIN");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .UseCollation("SQL_Latin1_General_CP850_BIN");

                entity.Property(e => e.StatusId).HasDefaultValueSql("((1))");

                entity.Property(e => e.Version).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.ProjectPackageCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProjectPackage_CreatedBy_UserAccount");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.ProjectPackageModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_ProjectPackage_ModifiedBy_UserAccount");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.ProjectPackages)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProjectPackage_Statuses");
            });

            modelBuilder.Entity<ProjectType>(entity =>
            {
                entity.ToTable("ProjectType");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Memo)
                    .HasMaxLength(500)
                    .UseCollation("SQL_Latin1_General_CP850_BIN");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .UseCollation("SQL_Latin1_General_CP850_BIN");

                entity.Property(e => e.StatusId).HasDefaultValueSql("((1))");

                entity.Property(e => e.Version).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.ProjectTypeCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProjectType_CreatedBy_UserAccount");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.ProjectTypeModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_ProjectType_ModifiedBy_UserAccount");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.ProjectTypes)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProjectType_Statuses");
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.KeyName)
                    .HasMaxLength(100)
                    .UseCollation("SQL_Latin1_General_CP850_BIN");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .UseCollation("SQL_Latin1_General_CP850_BIN");

                entity.Property(e => e.Version).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.ToTable("Ticket");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.StatusId).HasDefaultValueSql("((1))");

                entity.Property(e => e.Subject).HasMaxLength(100);

                entity.Property(e => e.Version).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TicketCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ticket_CreatedBy_UserAccount");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TicketModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_Ticket_ModifiedBy_UserAccount");

                entity.HasOne(d => d.Priority)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.PriorityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ticket_GlobalParam");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ticket_Project");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ticket_Statuses");

                entity.HasOne(d => d.TicketType)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.TicketTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ticket_TicketType");
            });

            modelBuilder.Entity<TicketAction>(entity =>
            {
                entity.ToTable("TicketAction");

                entity.Property(e => e.Description).UseCollation("SQL_Latin1_General_CP850_BIN");

                entity.Property(e => e.TransactionDate).HasColumnType("datetime");

                entity.HasOne(d => d.Ticket)
                    .WithMany(p => p.TicketActions)
                    .HasForeignKey(d => d.TicketId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TicketAction_Ticket");
            });

            modelBuilder.Entity<TicketActionFile>(entity =>
            {
                entity.ToTable("TicketActionFile");

                entity.Property(e => e.FileExtension)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .UseCollation("SQL_Latin1_General_CP850_BIN");

                entity.Property(e => e.FileName)
                    .HasMaxLength(200)
                    .UseCollation("SQL_Latin1_General_CP850_BIN");

                entity.HasOne(d => d.TicketAction)
                    .WithMany(p => p.TicketActionFiles)
                    .HasForeignKey(d => d.TicketActionId)
                    .HasConstraintName("FK_TicketActionFile_TicketAction");
            });

            modelBuilder.Entity<TicketType>(entity =>
            {
                entity.ToTable("TicketType");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Memo)
                    .HasMaxLength(500)
                    .UseCollation("SQL_Latin1_General_CP850_BIN");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .UseCollation("SQL_Latin1_General_CP850_BIN");

                entity.Property(e => e.StatusId).HasDefaultValueSql("((1))");

                entity.Property(e => e.Version).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TicketTypeCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TicketType_CreatedBy_UserAccount");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TicketTypeModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_TicketType_ModifiedBy_UserAccount");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.TicketTypes)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TicketType_Statuses");
            });

            modelBuilder.Entity<UserAccount>(entity =>
            {
                entity.ToTable("UserAccount");

                entity.Property(e => e.Address)
                    .HasMaxLength(500)
                    .UseCollation("SQL_Latin1_General_CP850_BIN");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .UseCollation("SQL_Latin1_General_CP850_BIN");

                entity.Property(e => e.FullName)
                    .HasMaxLength(100)
                    .UseCollation("SQL_Latin1_General_CP850_BIN");

                entity.Property(e => e.Memo)
                    .HasMaxLength(500)
                    .UseCollation("SQL_Latin1_General_CP850_BIN");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Phone)
                    .HasMaxLength(100)
                    .UseCollation("SQL_Latin1_General_CP850_BIN");

                entity.Property(e => e.StatusId).HasDefaultValueSql("((1))");

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .UseCollation("SQL_Latin1_General_CP850_BIN");

                entity.Property(e => e.Version).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.UserAccounts)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserAccount_Statuses");

                entity.HasOne(d => d.UserRole)
                    .WithMany(p => p.UserAccounts)
                    .HasForeignKey(d => d.UserRoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserAccount_UserRole");
            });

            modelBuilder.Entity<UserAccountToken>(entity =>
            {
                entity.ToTable("UserAccountToken");

                entity.HasOne(d => d.UserAccount)
                    .WithMany(p => p.UserAccountTokens)
                    .HasForeignKey(d => d.UserAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserAccountToken_UserAccount");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("UserRole");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Memo)
                    .HasMaxLength(500)
                    .UseCollation("SQL_Latin1_General_CP850_BIN");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .UseCollation("SQL_Latin1_General_CP850_BIN");

                entity.Property(e => e.StatusId).HasDefaultValueSql("((1))");

                entity.Property(e => e.Version).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserRole_Statuses");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
