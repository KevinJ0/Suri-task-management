using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Suri.Models
{
    public partial class MyUsers : IdentityUser
    {


        public virtual ICollection<Actividades> Actividades { get; set; }

    }
}
