using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using TaekwondoRanking.Repository.Models;

namespace TaekwondoRanking.Repository
{
    public partial class CompetitionsDbContext : DbContext
    {
        public CompetitionsDbContext()
        {
        }

        public CompetitionsDbContext(DbContextOptions<CompetitionsDbContext> options)
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
                optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\marti\\Competitions.mdf;Integrated Security=True;Connect Timeout=30");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Agekl>(entity =>
            {
                entity.HasKey(e => e.Idagekl)
                    .HasName("AGEKL$PrimaryKey");
            });

            modelBuilder.Entity<Athlet>(entity =>
            {
                entity.HasKey(e => e.Idathlet)
                    .HasName("ATHLETS$PrimaryKey");

                entity.Property(e => e.SsmaTimeStamp)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.HasOne(d => d.CountryNavigation)
                    .WithMany(p => p.Athlets)
                    .HasForeignKey(d => d.Country)
                    .HasConstraintName("ATHLETS$COUNTRYATHLETS");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Idctgr)
                    .HasName("CATEGORIES$PrimaryKey");

                entity.Property(e => e.SsmaTimeStamp)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.HasOne(d => d.AgeclassNavigation)
                    .WithMany(p => p.Categories)
                    .HasForeignKey(d => d.Ageclass)
                    .HasConstraintName("CATEGORIES$AGEKLCATEGORIES");
            });

            modelBuilder.Entity<Competition>(entity =>
            {
                entity.HasKey(e => e.Idcmpt)
                    .HasName("COMPETITIONS$PrimaryKey");

                entity.Property(e => e.Idcmpt).ValueGeneratedNever();

                entity.Property(e => e.SsmaTimeStamp)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.HasOne(d => d.CountryNavigation)
                    .WithMany(p => p.Competitions)
                    .HasForeignKey(d => d.Country)
                    .HasConstraintName("COMPETITIONS$COUNTRYCOMPETITIONS");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(e => e.Idcntr)
                    .HasName("COUNTRY$PrimaryKey");

                entity.Property(e => e.SsmaTimeStamp)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<Result>(entity =>
            {
                entity.HasKey(e => new { e.Idsubcmpt2, e.Idathlet })
                    .HasName("RESULTS$PrimaryKey");

                entity.Property(e => e.Place).HasDefaultValueSql("((0))");

                entity.Property(e => e.Points).HasDefaultValueSql("((0))");

                entity.Property(e => e.SsmaTimeStamp)
                    .IsRowVersion()
                    .IsConcurrencyToken();

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

                entity.Property(e => e.Idsubcmpt1).ValueGeneratedNever();

                entity.Property(e => e.Idcmpt).HasDefaultValueSql("((0))");

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

                entity.Property(e => e.Idsubcmpt2).ValueGeneratedNever();

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
