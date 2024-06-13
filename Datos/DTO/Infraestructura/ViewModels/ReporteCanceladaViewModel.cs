using System;
using System.ComponentModel.DataAnnotations;

namespace Datos.DTO.Infraestructura.ViewModels
{
    public class ReporteCanceladaViewModel
    {
        [Display(Name = @"Fecha Inicio")]
        public DateTime FechaInicio { get; set; }

        [Display(Name = @"Fecha Fin")]
        public DateTime FechaFin { get; set; }

        [Display(Name = @"Nombre del Solicitante")]
        public string Nombre { get; set; }

        [Display(Name = @"Apellido Paterno")]
        public string ApellidoP { get; set; }

        [Display(Name = @"Apellido Materno")]
        public string ApellidoM { get; set; }

        [Display(Name = @"Folio de Pago")]
        public string FolioDePago { get; set; }
    }
}
