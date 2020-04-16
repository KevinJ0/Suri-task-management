using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Suri.Models;

namespace Suri.Models
{
    public partial class SuriDbContext : IdentityDbContext<MyUsers>
    {
        public SuriDbContext()
        {
        }

        public SuriDbContext(DbContextOptions<SuriDbContext> options)
            : base(options)
        {
        }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           

            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database = suriDB;Trusted_connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // necessary for Identity
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Actividades>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Action)
                    .HasColumnName("action")
                    .HasMaxLength(120)
                    .IsUnicode(false);

                entity.Property(e => e.Actividad)
                    .IsRequired()
                    .HasColumnName("actividad")
                    .HasColumnType("text");

                entity.Property(e => e.Asistente)
                    .HasColumnName("asistente")
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Estado).HasColumnName("estado");

                entity.Property(e => e.FechaAsignacion)
                    .HasColumnName("fechaAsignacion")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FechaRealizacion)
                    .HasColumnName("fechaRealizacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.LocalidadId).HasColumnName("localidadId");

                entity.Property(e => e.Nota)
                    .HasColumnName("nota")
                    .HasColumnType("text");

                entity.Property(e => e.Prioridad).HasColumnName("prioridad");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("userId")
                    .HasMaxLength(450);

                entity.HasOne(d => d.Localidad)
                    .WithMany(p => p.Actividades)
                    .HasForeignKey(d => d.LocalidadId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Actividad__idloc__4AB81AF0");

                entity.HasOne(d => d.MyUser)
                    .WithMany(p => p.Actividades)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Actividad__iduse__49C3F6B7");
            });

            
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

        public DbSet<Suri.Models.Actividades> Actividades { get; set; }

        public DbSet<Suri.Models.Localidades> Localidades { get; set; }
    }
}
