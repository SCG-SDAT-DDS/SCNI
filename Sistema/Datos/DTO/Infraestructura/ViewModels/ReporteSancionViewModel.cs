using System;
using System.ComponentModel.DataAnnotations;

namespace Datos.DTO.Infraestructura.ViewModels
{
    public class ReporteSancionViewModel
    {
        [Display(Name = @"Fecha Inicio")]
        public DateTime FechaInicioResolucion { get; set; }
        [Display(Name = @"Fecha Fin")]
        public DateTime FechaFinResolucion { get; set; }
        [Display(Name = @"Fecha Inicio")]
        public DateTime FechaInicioEjecutoria { get; set; }
        [Display(Name = @"Fecha Fin")]
        public DateTime FechaFinEjecutoria { get; set; }
        [Display(Name = @"Fecha Inicio")]
        public DateTime FechaInicioInscripcion { get; set; }
        [Display(Name = @"Fecha Fin")]
        public DateTime FechaFinInscripcion { get; set; }
        [Display(Name = @"Tipo Sanción")]
        public int TipoSancion { get; set; }
        [Display(Name = @"Tipo Entidad")]
        public int TipoEntidad { get; set; }
        [Display(Name = @"Origen")]
        public int Origen { get; set; }
        public int ConSinObservaciones { get; set; }
    }
}
