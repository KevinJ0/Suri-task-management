using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Suri.Models
{
    public partial class Actividades
    {
        public int Id { get; set; }
        [Required]
        public string Actividad { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaAsignacion { get; set; }
        public string Nota { get; set; }
        public string Asistente { get; set; }
        [DataType(DataType.Date)]
        public DateTime? FechaRealizacion { get; set; }
        public string Action { get; set; }
        public string UserId { get; set; }
        public int LocalidadId { get; set; }
        [Required]
        public byte? Prioridad { get; set; }
        public bool? Estado { get; set; }

        public virtual Localidades Localidad { get; set; }
        public virtual MyUsers MyUser { get; set; }
    }
}
