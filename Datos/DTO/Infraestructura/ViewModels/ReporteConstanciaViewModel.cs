using System;
using System.ComponentModel.DataAnnotations;

namespace Datos.DTO.Infraestructura.ViewModels
{
    public class ReporteConstanciaViewModel
    {
        [Display(Name = @"Fecha Inicio")]
        public DateTime FechaInicio { get; set; }
        [Display(Name = @"Fecha Fin")]
        public DateTime FechaFin { get; set; }
        [Display(Name = @"Tipo Solicitud")]
        public int TipoSolicitud { get; set; }
        [Display(Name = @"Medio")]
        public int Medio { get; set; }
        [Display(Name = @"Tipo Sanción")]
        public int TipoSancion { get; set; }
        [Display(Name = @"Tipo Entidad")]
        public int TipoEntidad { get; set; }
        public int ConSinSancion { get; set; }
    }
}
