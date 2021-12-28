
using Microsoft.EntityFrameworkCore;
using BokButikLab03.Models;
using Microsoft.Extensions.Configuration;

namespace BokButikLab03.Data
{




    public partial class Laboration2RBContext : DbContext
    {

        public Laboration2RBContext()
        {
        }

        public Laboration2RBContext(DbContextOptions<Laboration2RBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Butiker> Butikers { get; set; } = null!;
        public virtual DbSet<Böcker> Böckers { get; set; } = null!;
        public virtual DbSet<Författare> Författares { get; set; } = null!;
        public virtual DbSet<Förlag> Förlags { get; set; } = null!;
        public virtual DbSet<LagerSaldo> LagerSaldos { get; set; } = null!;

  


        
       protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true);
                string connectionString =
                builder.Build().GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
    
            }
        }
     

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Böcker>(entity =>
            {
                entity.HasKey(e => e.Isbn13)
                    .HasName("PK__Böcker__3BF79E037265D8E8");

                entity.Property(e => e.Isbn13).ValueGeneratedNever();
            });

            modelBuilder.Entity<Författare>(entity =>
            {
                entity.HasMany(d => d.BöckerIsbns)
                    .WithMany(p => p.Författares)
                    .UsingEntity<Dictionary<string, object>>(
                        "FörfattareBöcker",
                        l => l.HasOne<Böcker>().WithMany().HasForeignKey("BöckerIsbn").OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK__Författar__Böcke__440B1D61"),
                        r => r.HasOne<Författare>().WithMany().HasForeignKey("FörfattareId").OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK__Författar__Förfa__4316F928"),
                        j =>
                        {
                            j.HasKey("FörfattareId", "BöckerIsbn").HasName("PK_FB");

                            j.ToTable("FörfattareBöcker");

                            j.IndexerProperty<int>("FörfattareId").HasColumnName("FörfattareID");

                            j.IndexerProperty<long>("BöckerIsbn").HasColumnName("BöckerISBN");
                        });
            });

            modelBuilder.Entity<Förlag>(entity =>
            {
                entity.HasOne(d => d.BokIsbn13Navigation)
                    .WithMany(p => p.Förlags)
                    .HasForeignKey(d => d.BokIsbn13)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Förlag__BokISBN1__38996AB5");
            });

            modelBuilder.Entity<LagerSaldo>(entity =>
            {
                entity.HasKey(e => new { e.ButikId, e.Isbn });

                entity.HasOne(d => d.Butik)
                    .WithMany(p => p.LagerSaldos)
                    .HasForeignKey(d => d.ButikId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__LagerSald__Butik__2A4B4B5E");

                entity.HasOne(d => d.IsbnNavigation)
                    .WithMany(p => p.LagerSaldos)
                    .HasForeignKey(d => d.Isbn)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__LagerSaldo__ISBN__2B3F6F97");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
