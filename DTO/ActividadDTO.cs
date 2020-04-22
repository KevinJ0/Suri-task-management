using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Suri.DTO
{
    public partial class ActividadDTO
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Síntoma")]
        public string Actividad { get; set; }
        [Required]
        [DisplayName("Fecha de Asignación")]
        [DataType(DataType.Date)]
        public DateTime FechaAsignacion { get; set; }
        public string Nota { get; set; }
        public string Asistente { get; set; }
        [DisplayName("Fecha de Realización")]
        [DataType(DataType.Date)]
        public DateTime? FechaRealizacion { get; set; }
        [DisplayName("Acción")]
        public string accion { get; set; }
        public string MyUserId { get; set; }
        public int LocalidadId { get; set; }
        public int PrioridadId { get; set; }
        public bool Estado { set; get; }

        public virtual Localidades Localidad { get; set; }
        public virtual Prioridades Prioridad { get; set; }
        [DisplayName("Técnico encargado")]
        public virtual MyUsers MyUser { get; set; }
    }
}
