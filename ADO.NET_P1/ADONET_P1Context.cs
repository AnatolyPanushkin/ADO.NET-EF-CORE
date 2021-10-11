using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ADO.NET_P1
{
    public partial class ADONET_P1Context : DbContext
    {
        public ADONET_P1Context()
        {
        }

        public ADONET_P1Context(DbContextOptions<ADONET_P1Context> options)
            : base(options)
        {
        }

        public virtual DbSet<B> Bs { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<EvilnessFactor> EvilnessFactors { get; set; }
        public virtual DbSet<Minion> Minions { get; set; }
        public virtual DbSet<MinionVillain> MinionVillains { get; set; }
        public virtual DbSet<Town> Towns { get; set; }
        public virtual DbSet<V> Vs { get; set; }
        public virtual DbSet<V1> V1s { get; set; }
        public virtual DbSet<Villain> Villains { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.;Database=ADO.NET_P1;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<B>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("b");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<EvilnessFactor>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Minion>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Town)
                    .WithMany(p => p.Minions)
                    .HasForeignKey(d => d.TownId)
                    .HasConstraintName("FK_Minions_Towns");
            });

            modelBuilder.Entity<MinionVillain>(entity =>
            {
                entity.HasKey(e => new { e.MinionId, e.VillainId });

                entity.ToTable("MinionVillain");

                entity.HasOne(d => d.Minion)
                    .WithMany(p => p.MinionVillains)
                    .HasForeignKey(d => d.MinionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MinionVillain_Minions");

                entity.HasOne(d => d.Villain)
                    .WithMany(p => p.MinionVillains)
                    .HasForeignKey(d => d.VillainId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MinionVillain_Villains");
            });

            modelBuilder.Entity<Town>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsFixedLength(true);

                entity.HasOne(d => d.CountryCodeNavigation)
                    .WithMany(p => p.Towns)
                    .HasForeignKey(d => d.CountryCode)
                    .HasConstraintName("FK_Towns_Countries");
            });

            modelBuilder.Entity<V>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("v");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<V1>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("v1");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Villain>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.EvilnessFactor)
                    .WithMany(p => p.Villains)
                    .HasForeignKey(d => d.EvilnessFactorId)
                    .HasConstraintName("FK_Villains_EvilnessFactors");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
