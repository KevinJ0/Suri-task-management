using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Suri.Models
{
    public partial class SuriDBContext : DbContext
    {
        public SuriDBContext()
        {
        }

        public SuriDBContext(DbContextOptions<SuriDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Localidades> Localidades { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database = SuriDB;Trusted_connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Localidades>(entity =>
            {
                entity.HasIndex(e => e.CeldaId)
                    .HasName("UQ__Localida__45F5B5D34D381008")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CeldaId)
                    .HasColumnName("celdaId")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Coordenadas)
                    .HasColumnName("coordenadas")
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("nombre")
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });
        }
    }
}
