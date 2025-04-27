using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TaekwondoRanking.Models
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Agekl> Agekls { get; set; } = null!;
        public virtual DbSet<Athlet> Athlets { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Competition> Competitions { get; set; } = null!;
        public virtual DbSet<Country> Countries { get; set; } = null!;
        public virtual DbSet<Result> Results { get; set; } = null!;
        public virtual DbSet<Subcmpt1> Subcmpt1s { get; set; } = null!;
        public virtual DbSet<Subcmpt2> Subcmpt2s { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=Competitions;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Agekl>(entity =>
            {
                entity.HasKey(e => e.Idagekl)
                    .HasName("AGEKL$PrimaryKey");

                entity.ToTable("AGEKL");

                entity.Property(e => e.Idagekl)
                    .HasMaxLength(3)
                    .HasColumnName("IDAGEKL");

                entity.Property(e => e.Nameagekl)
                    .HasMaxLength(255)
                    .HasColumnName("NAMEAGEKL");
            });

            modelBuilder.Entity<Athlet>(entity =>
            {
                entity.HasKey(e => e.Idathlet)
                    .HasName("ATHLETS$PrimaryKey");

                entity.ToTable("ATHLETS");

                entity.Property(e => e.Idathlet)
                    .HasMaxLength(10)
                    .HasColumnName("IDATHLET");

                entity.Property(e => e.Country)
                    .HasMaxLength(3)
                    .HasColumnName("COUNTRY");

                entity.Property(e => e.Name).HasColumnName("NAME");

                entity.Property(e => e.SsmaTimeStamp)
                    .IsRowVersion()
                    .IsConcurrencyToken()
                    .HasColumnName("SSMA_TimeStamp");

                entity.HasOne(d => d.CountryNavigation)
                    .WithMany(p => p.Athlets)
                    .HasForeignKey(d => d.Country)
                    .HasConstraintName("ATHLETS$COUNTRYATHLETS");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Idctgr)
                    .HasName("CATEGORIES$PrimaryKey");

                entity.ToTable("CATEGORIES");

                entity.Property(e => e.Idctgr)
                    .HasMaxLength(12)
                    .HasColumnName("IDCTGR");

                entity.Property(e => e.Ageclass)
                    .HasMaxLength(3)
                    .HasColumnName("AGECLASS");

                entity.Property(e => e.Mf)
                    .HasMaxLength(255)
                    .HasColumnName("MF");

                entity.Property(e => e.Namectgr).HasColumnName("NAMECTGR");

                entity.Property(e => e.SsmaTimeStamp)
                    .IsRowVersion()
                    .IsConcurrencyToken()
                    .HasColumnName("SSMA_TimeStamp");

                entity.HasOne(d => d.AgeclassNavigation)
                    .WithMany(p => p.Categories)
                    .HasForeignKey(d => d.Ageclass)
                    .HasConstraintName("CATEGORIES$AGEKLCATEGORIES");
            });

            modelBuilder.Entity<Competition>(entity =>
            {
                entity.HasKey(e => e.Idcmpt)
                    .HasName("COMPETITIONS$PrimaryKey");

                entity.ToTable("COMPETITIONS");

                entity.HasIndex(e => e.Country, "COMPETITIONS$IDCOUNT");

                entity.Property(e => e.Idcmpt)
                    .ValueGeneratedNever()
                    .HasColumnName("IDCMPT");

                entity.Property(e => e.Country)
                    .HasMaxLength(3)
                    .HasColumnName("COUNTRY");

                entity.Property(e => e.Fromdate)
                    .HasPrecision(0)
                    .HasColumnName("FROMDATE");

                entity.Property(e => e.Namecmpt).HasColumnName("NAMECMPT");

                entity.Property(e => e.Rangelabel).HasColumnName("RANGELABEL");

                entity.Property(e => e.SsmaTimeStamp)
                    .IsRowVersion()
                    .IsConcurrencyToken()
                    .HasColumnName("SSMA_TimeStamp");

                entity.Property(e => e.Tilldate)
                    .HasPrecision(0)
                    .HasColumnName("TILLDATE");

                entity.HasOne(d => d.CountryNavigation)
                    .WithMany(p => p.Competitions)
                    .HasForeignKey(d => d.Country)
                    .HasConstraintName("COMPETITIONS$COUNTRYCOMPETITIONS");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(e => e.Idcntr)
                    .HasName("COUNTRY$PrimaryKey");

                entity.ToTable("COUNTRY");

                entity.Property(e => e.Idcntr)
                    .HasMaxLength(3)
                    .HasColumnName("IDCNTR");

                entity.Property(e => e.Namecntr).HasColumnName("NAMECNTR");

                entity.Property(e => e.SsmaTimeStamp)
                    .IsRowVersion()
                    .IsConcurrencyToken()
                    .HasColumnName("SSMA_TimeStamp");
            });

            modelBuilder.Entity<Result>(entity =>
            {
                entity.HasKey(e => new { e.Idsubcmpt2, e.Idathlet })
                    .HasName("RESULTS$PrimaryKey");

                entity.ToTable("RESULTS");

                entity.HasIndex(e => e.Idathlet, "RESULTS$IDATHLET");

                entity.Property(e => e.Idsubcmpt2).HasColumnName("IDSUBCMPT2");

                entity.Property(e => e.Idathlet)
                    .HasMaxLength(10)
                    .HasColumnName("IDATHLET");

                entity.Property(e => e.Place)
                    .HasColumnName("PLACE")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Points)
                    .HasColumnName("POINTS")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.SsmaTimeStamp)
                    .IsRowVersion()
                    .IsConcurrencyToken()
                    .HasColumnName("SSMA_TimeStamp");

                entity.HasOne(d => d.IdathletNavigation)
                    .WithMany(p => p.Results)
                    .HasForeignKey(d => d.Idathlet)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("RESULTS$ATHLETSCOMPETITORS");

                entity.HasOne(d => d.Idsubcmpt2Navigation)
                    .WithMany(p => p.Results)
                    .HasForeignKey(d => d.Idsubcmpt2)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("RESULTS$SUBCMPT2RESULTS");
            });

            modelBuilder.Entity<Subcmpt1>(entity =>
            {
                entity.HasKey(e => e.Idsubcmpt1)
                    .HasName("SUBCMPT1$PrimaryKey");

                entity.ToTable("SUBCMPT1");

                entity.HasIndex(e => e.Idcmpt, "SUBCMPT1$IDCMPT");

                entity.HasIndex(e => e.Idsubcmpt1, "SUBCMPT1$IDSUBCMPT1");

                entity.Property(e => e.Idsubcmpt1)
                    .ValueGeneratedNever()
                    .HasColumnName("IDSUBCMPT1");

                entity.Property(e => e.Ageclass)
                    .HasMaxLength(3)
                    .HasColumnName("AGECLASS");

                entity.Property(e => e.Idcmpt)
                    .HasColumnName("IDCMPT")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Playdate)
                    .HasPrecision(0)
                    .HasColumnName("PLAYDATE");

                entity.Property(e => e.Rank)
                    .HasMaxLength(3)
                    .HasColumnName("RANK");

                entity.HasOne(d => d.AgeclassNavigation)
                    .WithMany(p => p.Subcmpt1s)
                    .HasForeignKey(d => d.Ageclass)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SUBCMPT1$AGEKLSUBCMPT1");

                entity.HasOne(d => d.IdcmptNavigation)
                    .WithMany(p => p.Subcmpt1s)
                    .HasForeignKey(d => d.Idcmpt)
                    .HasConstraintName("SUBCMPT1$COMPETITIONSSUBCMPT1");
            });

            modelBuilder.Entity<Subcmpt2>(entity =>
            {
                entity.HasKey(e => e.Idsubcmpt2)
                    .HasName("SUBCMPT2$PrimaryKey");

                entity.ToTable("SUBCMPT2");

                entity.HasIndex(e => e.Idsubcmpt1, "SUBCMPT2$IDSUBCMPT1");

                entity.Property(e => e.Idsubcmpt2)
                    .ValueGeneratedNever()
                    .HasColumnName("IDSUBCMPT2");

                entity.Property(e => e.Idctgr)
                    .HasMaxLength(12)
                    .HasColumnName("IDCTGR");

                entity.Property(e => e.Idsubcmpt1).HasColumnName("IDSUBCMPT1");

                entity.HasOne(d => d.IdctgrNavigation)
                    .WithMany(p => p.Subcmpt2s)
                    .HasForeignKey(d => d.Idctgr)
                    .HasConstraintName("SUBCMPT2$CATEGORIESSUBCMPT2");

                entity.HasOne(d => d.Idsubcmpt1Navigation)
                    .WithMany(p => p.Subcmpt2s)
                    .HasForeignKey(d => d.Idsubcmpt1)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SUBCMPT2$SUBCMPT1SUBCMPT2");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
