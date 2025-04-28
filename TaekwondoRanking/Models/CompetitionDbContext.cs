using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TaekwondoRanking.Models
{
    public partial class CompetitionDbContext : DbContext
    {
        public CompetitionDbContext()
        {
        }

        public CompetitionDbContext(DbContextOptions<CompetitionDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AgeClass> AgeClasses { get; set; } = null!;
        public virtual DbSet<Athlete> Athletes { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Competition> Competitions { get; set; } = null!;
        public virtual DbSet<Country> Countries { get; set; } = null!;
        public virtual DbSet<Result> Results { get; set; } = null!;
        public virtual DbSet<SubCompetition1> SubCompetition1s { get; set; } = null!;
        public virtual DbSet<SubCompetition2> SubCompetition2s { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AgeClass>(entity =>
            {
                entity.HasKey(e => e.IdAgeClass)
                    .HasName("AGEKL$PrimaryKey");

                entity.ToTable("AgeClass");

                entity.Property(e => e.IdAgeClass).HasMaxLength(3);

                entity.Property(e => e.NameAgeClass).HasMaxLength(255);
            });

            modelBuilder.Entity<Athlete>(entity =>
            {
                entity.HasKey(e => e.IdAthlete)
                    .HasName("ATHLETS$PrimaryKey");

                entity.Property(e => e.IdAthlete).HasMaxLength(10);

                entity.Property(e => e.Country).HasMaxLength(3);

                entity.HasOne(d => d.CountryNavigation)
                    .WithMany(p => p.Athletes)
                    .HasForeignKey(d => d.Country)
                    .HasConstraintName("ATHLETS$COUNTRYATHLETS");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.IdCategory)
                    .HasName("CATEGORIES$PrimaryKey");

                entity.Property(e => e.IdCategory).HasMaxLength(12);

                entity.Property(e => e.AgeClass).HasMaxLength(3);

                entity.Property(e => e.Mf)
                    .HasMaxLength(255)
                    .HasColumnName("MF");

                entity.HasOne(d => d.AgeClassNavigation)
                    .WithMany(p => p.Categories)
                    .HasForeignKey(d => d.AgeClass)
                    .HasConstraintName("CATEGORIES$AGEKLCATEGORIES");
            });

            modelBuilder.Entity<Competition>(entity =>
            {
                entity.HasKey(e => e.IdCompetition)
                    .HasName("COMPETITIONS$PrimaryKey");

                entity.HasIndex(e => e.Country, "COMPETITIONS$IDCOUNT");

                entity.Property(e => e.IdCompetition).ValueGeneratedNever();

                entity.Property(e => e.Country).HasMaxLength(3);

                entity.Property(e => e.FromDate).HasPrecision(0);

                entity.Property(e => e.TillDate).HasPrecision(0);

                entity.HasOne(d => d.CountryNavigation)
                    .WithMany(p => p.Competitions)
                    .HasForeignKey(d => d.Country)
                    .HasConstraintName("COMPETITIONS$COUNTRYCOMPETITIONS");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(e => e.IdCountry)
                    .HasName("COUNTRY$PrimaryKey");

                entity.ToTable("Country");

                entity.Property(e => e.IdCountry).HasMaxLength(3);
            });

            modelBuilder.Entity<Result>(entity =>
            {
                entity.HasKey(e => new { e.IdSubCompetition2, e.IdAthlete })
                    .HasName("RESULTS$PrimaryKey");

                entity.HasIndex(e => e.IdAthlete, "RESULTS$IDATHLET");

                entity.Property(e => e.IdAthlete).HasMaxLength(10);

                entity.Property(e => e.Place).HasDefaultValueSql("((0))");

                entity.Property(e => e.Points).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.IdAthleteNavigation)
                    .WithMany(p => p.Results)
                    .HasForeignKey(d => d.IdAthlete)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("RESULTS$ATHLETSCOMPETITORS");

                entity.HasOne(d => d.IdSubCompetition2Navigation)
                    .WithMany(p => p.Results)
                    .HasForeignKey(d => d.IdSubCompetition2)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("RESULTS$SUBCMPT2RESULTS");
            });

            modelBuilder.Entity<SubCompetition1>(entity =>
            {
                entity.HasKey(e => e.IdSubCompetition1)
                    .HasName("SUBCMPT1$PrimaryKey");

                entity.ToTable("SubCompetition1");

                entity.HasIndex(e => e.IdCompetition, "SUBCMPT1$IDCMPT");

                entity.HasIndex(e => e.IdSubCompetition1, "SUBCMPT1$IDSUBCMPT1");

                entity.Property(e => e.IdSubCompetition1).ValueGeneratedNever();

                entity.Property(e => e.AgeClass).HasMaxLength(3);

                entity.Property(e => e.IdCompetition).HasDefaultValueSql("((0))");

                entity.Property(e => e.PlayDate).HasPrecision(0);

                entity.Property(e => e.Rank).HasMaxLength(3);

                entity.HasOne(d => d.AgeClassNavigation)
                    .WithMany(p => p.SubCompetition1s)
                    .HasForeignKey(d => d.AgeClass)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SUBCMPT1$AGEKLSUBCMPT1");

                entity.HasOne(d => d.IdCompetitionNavigation)
                    .WithMany(p => p.SubCompetition1s)
                    .HasForeignKey(d => d.IdCompetition)
                    .HasConstraintName("SUBCMPT1$COMPETITIONSSUBCMPT1");
            });

            modelBuilder.Entity<SubCompetition2>(entity =>
            {
                entity.HasKey(e => e.IdSubCompetition2)
                    .HasName("SUBCMPT2$PrimaryKey");

                entity.ToTable("SubCompetition2");

                entity.HasIndex(e => e.IdSubCompetition1, "SUBCMPT2$IDSUBCMPT1");

                entity.Property(e => e.IdSubCompetition2).ValueGeneratedNever();

                entity.Property(e => e.IdCategory).HasMaxLength(12);

                entity.HasOne(d => d.IdCategoryNavigation)
                    .WithMany(p => p.SubCompetition2s)
                    .HasForeignKey(d => d.IdCategory)
                    .HasConstraintName("SUBCMPT2$CATEGORIESSUBCMPT2");

                entity.HasOne(d => d.IdSubCompetition1Navigation)
                    .WithMany(p => p.SubCompetition2s)
                    .HasForeignKey(d => d.IdSubCompetition1)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SUBCMPT2$SUBCMPT1SUBCMPT2");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
