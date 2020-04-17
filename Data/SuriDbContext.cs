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

          
          
        }

        public DbSet<Suri.Models.Actividades> Actividades { get; set; }

        public DbSet<Suri.Models.Localidades> Localidades { get; set; }
        public DbSet<Suri.Models.Prioridades> Prioridades { get; set; }
    }
}
