using System;
using System.Collections.Generic;

namespace Datos.DTO.Infraestructura.ViewModels
{
    public class SancionViewModel : PaginacionViewModel
    {
        public Sancion Sancion { get; set; }
        public List<Sancion> Sanciones { get; set; }
        public bool? EsEstatal { get; set; }
        public string Imprimir { get; set; }
        public DateTime? FechaInicioEjecutaria { get; set; }
        public DateTime? FechaFinEjecutaria { get; set; }
        public DateTime? FechaInicioResolucion { get; set; }
        public DateTime? FechaFinResolucion { get; set; }
    }
}
