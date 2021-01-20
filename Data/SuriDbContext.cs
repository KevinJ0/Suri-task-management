using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Suri.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Suri.Data
{
    public class SuriDbContext : IdentityDbContext<MyUsers>
    {


        public SuriDbContext(DbContextOptions<SuriDbContext> options) : base(options)
        {


        }

        public DbSet<Actividades> Actividades { get; set; }
        public DbSet<Localidades> Localidades { get; set; }
         public DbSet<Prioridades> Prioridades { get; set; }

    }


}
