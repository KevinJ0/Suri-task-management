using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Suri.Models
{
    public partial class Localidades
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Ingrese un nombre")]
        public string Nombre { get; set; }
        public string Coordenadas { get; set; }
        public string CeldaId { get; set; }
    }
}
