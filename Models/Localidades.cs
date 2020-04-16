using System;
using System.Collections.Generic;

namespace Suri.Models
{
    public partial class Localidades
    {
        public Localidades()
        {
            Actividades = new HashSet<Actividades>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Coordenadas { get; set; }
        public string CeldaId { get; set; }

        public virtual ICollection<Actividades> Actividades { get; set; }
    }
}
