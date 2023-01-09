using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using MSysICTSBM.Dal.DataContexts.Models.DB.MainModels;

#nullable disable

namespace MSysICTSBM.Dal.DataContexts.Models.DB.MainContext
{
    public partial class MSysMainEntities : DbContext
    {
        public MSysMainEntities()
        {
        }

        public MSysMainEntities(DbContextOptions<MSysMainEntities> options)
            : base(options)
        {
        }

        public virtual DbSet<DocMaster> DocMasters { get; set; }
        public virtual DbSet<DocSubMaster> DocSubMasters { get; set; }
        public virtual DbSet<EmployeeMaster> EmployeeMasters { get; set; }
        public virtual DbSet<QrPrinted> QrPrinteds { get; set; }
        public virtual DbSet<QrReceive> QrReceives { get; set; }
        public virtual DbSet<QrSent> QrSents { get; set; }
        public virtual DbSet<ULB_App_Status> ULB_App_Statuses { get; set; }
        public virtual DbSet<ULB_Detail> ULB_Details { get; set; }
        public virtual DbSet<ULB_DigCopy_Rec> ULB_DigCopy_Recs { get; set; }
        public virtual DbSet<ULB_Doc_Send> ULB_Doc_Sends { get; set; }
        public virtual DbSet<ULB_HardCopy_Rec> ULB_HardCopy_Recs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("data source=202.65.157.254;initial catalog=ManangementSystemICTSBM;persist security info=True;user id=appynitty;password=BigV$Telecom;MultipleActiveResultSets=True;App=EntityFramework");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AI");

            modelBuilder.Entity<DocMaster>(entity =>
            {
                entity.ToTable("DocMaster");

                entity.Property(e => e.DocDate).HasColumnType("datetime");

                entity.Property(e => e.DocName).HasMaxLength(100);
            });

            modelBuilder.Entity<DocSubMaster>(entity =>
            {
                entity.ToTable("DocSubMaster");

                entity.Property(e => e.DocSubDate).HasColumnType("datetime");

                entity.Property(e => e.DocSubName).HasMaxLength(100);
            });

            modelBuilder.Entity<EmployeeMaster>(entity =>
            {
                entity.ToTable("EmployeeMaster");

                entity.Property(e => e.Address).HasMaxLength(200);

                entity.Property(e => e.Create_Date)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MobileNumber).HasMaxLength(20);

                entity.Property(e => e.Name).HasMaxLength(500);

                entity.Property(e => e.Password).HasMaxLength(100);

                entity.Property(e => e.Type).HasMaxLength(50);

                entity.Property(e => e.Username).HasMaxLength(200);

                entity.Property(e => e.imoNo).HasMaxLength(200);

                entity.Property(e => e.lastModifyDateEntry).HasColumnType("datetime");
            });

            modelBuilder.Entity<QrPrinted>(entity =>
            {
                entity.HasKey(e => e.PrintId);

                entity.ToTable("QrPrinted");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.Note).HasMaxLength(500);

                entity.Property(e => e.PrintDate).HasColumnType("datetime");

                entity.Property(e => e.UpdationDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<QrReceive>(entity =>
            {
                entity.HasKey(e => e.ReceiveId);

                entity.ToTable("QrReceive");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.Note).HasMaxLength(500);

                entity.Property(e => e.ReceiveDate).HasColumnType("datetime");

                entity.Property(e => e.UpdationDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<QrSent>(entity =>
            {
                entity.HasKey(e => e.SentId);

                entity.ToTable("QrSent");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.Note).HasMaxLength(500);

                entity.Property(e => e.SentDate).HasColumnType("datetime");

                entity.Property(e => e.UpdationDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<ULB_App_Status>(entity =>
            {
                entity.ToTable("ULB_App_Status");

                entity.Property(e => e.AppDate).HasColumnType("datetime");

                entity.Property(e => e.CMSDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<ULB_Detail>(entity =>
            {
                entity.Property(e => e.AppName).HasMaxLength(250);

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<ULB_DigCopy_Rec>(entity =>
            {
                entity.ToTable("ULB_DigCopy_Rec");

                entity.Property(e => e.DocCreateDate).HasColumnType("datetime");

                entity.Property(e => e.DocUpdateDate).HasColumnType("datetime");

                entity.Property(e => e.Note).HasMaxLength(500);
            });

            modelBuilder.Entity<ULB_Doc_Send>(entity =>
            {
                entity.ToTable("ULB_Doc_Send");

                entity.Property(e => e.DocCreateDate).HasColumnType("datetime");

                entity.Property(e => e.DocUpdateDate).HasColumnType("datetime");

                entity.Property(e => e.Note).HasMaxLength(500);
            });

            modelBuilder.Entity<ULB_HardCopy_Rec>(entity =>
            {
                entity.ToTable("ULB_HardCopy_Rec");

                entity.Property(e => e.DocCreateDate).HasColumnType("datetime");

                entity.Property(e => e.DocUpdateDate).HasColumnType("datetime");

                entity.Property(e => e.Note).HasMaxLength(500);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
